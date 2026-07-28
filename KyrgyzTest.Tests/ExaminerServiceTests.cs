using KyrgyzTest.Application.DTOs;
using KyrgyzTest.Application.Interfaces;
using KyrgyzTest.Application.Services;
using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Enums;
using KyrgyzTest.Core.Exceptions;
using KyrgyzTest.Core.Interfaces;
using NSubstitute;
using Xunit;

namespace KyrgyzTest.Tests;

public class ExaminerServiceTests
{
    private readonly IAttemptRepository _attemptRepo = Substitute.For<IAttemptRepository>();
    private readonly ICandidateAnswerRepository _answerRepo = Substitute.For<ICandidateAnswerRepository>();
    private readonly IManualGradeRepository _manualGradeRepo = Substitute.For<IManualGradeRepository>();
    private readonly IExamService _examService = Substitute.For<IExamService>();

    private ExaminerService BuildService() => new(_attemptRepo, _answerRepo, _manualGradeRepo, _examService);

    private static Attempt BuildCompletedAttempt(out Guid openAnswerQuestionId)
        => BuildCompletedAttempt(out openAnswerQuestionId, includeMcq: false, out _);

    private static Attempt BuildCompletedAttempt(
        out Guid openAnswerQuestionId,
        bool includeMcq,
        out Guid mcqQuestionId)
    {
        openAnswerQuestionId = Guid.NewGuid();
        mcqQuestionId = Guid.NewGuid();

        var openAnswerQuestion = new Question
        {
            Id = openAnswerQuestionId,
            Section = SectionType.Speaking,
            Level = LanguageLevel.A1,
            Type = QuestionType.OpenAnswer,
            Content = "Describe your day",
            AnswerOptions = new List<AnswerOption>(),
        };

        var questions = new List<TestVariantQuestion>
        {
            new() { Id = Guid.NewGuid(), QuestionId = openAnswerQuestionId, Question = openAnswerQuestion, OrderIndex = 0 },
        };

        if (includeMcq)
        {
            var mcqQuestion = new Question
            {
                Id = mcqQuestionId,
                Section = SectionType.Grammar,
                Level = LanguageLevel.A1,
                Type = QuestionType.MCQ,
                Content = "Pick one",
                AnswerOptions = new List<AnswerOption>(),
            };
            questions.Add(new() { Id = Guid.NewGuid(), QuestionId = mcqQuestionId, Question = mcqQuestion, OrderIndex = 1 });
        }

        var variant = new TestVariant { Id = Guid.NewGuid(), Number = 1, Questions = questions };
        var candidate = new Candidate
        {
            Id = Guid.NewGuid(),
            FullName = "Test Candidate",
            Inn = "12345678901234",
            AccessCode = "20260515-1234",
            IsAllowed = true
        };

        return new Attempt
        {
            Id = Guid.NewGuid(),
            CandidateId = candidate.Id,
            Candidate = candidate,
            TestVariantId = variant.Id,
            TestVariant = variant,
            Status = AttemptStatus.Completed,
            SubmittedAt = DateTime.UtcNow,
        };
    }

    [Fact]
    public async Task GetQueueAsync_UngradedOpenAnswer_AppearsInQueue()
    {
        var attempt = BuildCompletedAttempt(out _);
        _attemptRepo.GetCompletedWithDetailsAsync().Returns(new List<Attempt> { attempt });
        _manualGradeRepo.GetByAttemptIdAsync(attempt.Id).Returns(new List<ManualGrade>());

        var queue = await BuildService().GetQueueAsync();

        var item = Assert.Single(queue);
        Assert.Equal(attempt.Id, item.AttemptId);
        Assert.Equal(attempt.Candidate.FullName, item.CandidateFullName);
        Assert.Equal(1, item.UngradedCount);
    }

    [Fact]
    public async Task GetQueueAsync_FullyGradedAttempt_IsExcluded()
    {
        var attempt = BuildCompletedAttempt(out var questionId);
        _attemptRepo.GetCompletedWithDetailsAsync().Returns(new List<Attempt> { attempt });
        _manualGradeRepo.GetByAttemptIdAsync(attempt.Id).Returns(new List<ManualGrade>
        {
            new() { Id = Guid.NewGuid(), AttemptId = attempt.Id, QuestionId = questionId, Score = 3, GradedByUserId = Guid.NewGuid(), GradedAt = DateTime.UtcNow },
        });

        var queue = await BuildService().GetQueueAsync();

        Assert.Empty(queue);
    }

    [Fact]
    public async Task GetQueueAsync_AttemptWithNoOpenAnswerQuestions_IsExcluded()
    {
        var attempt = BuildCompletedAttempt(out _, includeMcq: true, out _);
        attempt.TestVariant.Questions = attempt.TestVariant.Questions
            .Where(q => q.Question.Type != QuestionType.OpenAnswer)
            .ToList();
        _attemptRepo.GetCompletedWithDetailsAsync().Returns(new List<Attempt> { attempt });

        var queue = await BuildService().GetQueueAsync();

        Assert.Empty(queue);
        await _manualGradeRepo.DidNotReceive().GetByAttemptIdAsync(Arg.Any<Guid>());
    }

    [Fact]
    public async Task GetReviewAsync_ReturnsOnlyOpenAnswerQuestions_WithAudioUrlAndExistingScore()
    {
        var attempt = BuildCompletedAttempt(out var openAnswerQuestionId, includeMcq: true, out var mcqQuestionId);
        _attemptRepo.GetByIdWithDetailsAsync(attempt.Id).Returns(attempt);
        _answerRepo.GetByAttemptIdAsync(attempt.Id).Returns(new List<CandidateAnswer>
        {
            new() { Id = Guid.NewGuid(), AttemptId = attempt.Id, QuestionId = openAnswerQuestionId, AudioAnswerUrl = "/uploads/a.webm" },
            new() { Id = Guid.NewGuid(), AttemptId = attempt.Id, QuestionId = mcqQuestionId, SelectedOptionId = Guid.NewGuid() },
        });
        _manualGradeRepo.GetByAttemptIdAsync(attempt.Id).Returns(new List<ManualGrade>
        {
            new() { Id = Guid.NewGuid(), AttemptId = attempt.Id, QuestionId = openAnswerQuestionId, Score = 4, GradedByUserId = Guid.NewGuid(), GradedAt = DateTime.UtcNow },
        });

        var review = await BuildService().GetReviewAsync(attempt.Id);

        var answer = Assert.Single(review.Answers);
        Assert.Equal(openAnswerQuestionId, answer.QuestionId);
        Assert.Equal("/uploads/a.webm", answer.AudioAnswerUrl);
        Assert.Equal(4, answer.Score);
    }

    [Fact]
    public async Task GetReviewAsync_AttemptNotFound_Throws()
    {
        var attemptId = Guid.NewGuid();
        _attemptRepo.GetByIdWithDetailsAsync(attemptId).Returns((Attempt?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => BuildService().GetReviewAsync(attemptId));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(6)]
    public async Task GradeAsync_ScoreOutOfRange_ThrowsValidationException(int score)
    {
        var dto = new GradeAnswerDto { AttemptId = Guid.NewGuid(), QuestionId = Guid.NewGuid(), Score = score };

        await Assert.ThrowsAsync<ValidationException>(() => BuildService().GradeAsync(dto, Guid.NewGuid()));

        await _manualGradeRepo.DidNotReceive().UpsertAsync(Arg.Any<ManualGrade>());
    }

    [Fact]
    public async Task GradeAsync_QuestionNotOpenAnswer_ThrowsBusinessException()
    {
        var attempt = BuildCompletedAttempt(out _, includeMcq: true, out var mcqQuestionId);
        _attemptRepo.GetByIdWithDetailsAsync(attempt.Id).Returns(attempt);
        var dto = new GradeAnswerDto { AttemptId = attempt.Id, QuestionId = mcqQuestionId, Score = 3 };

        await Assert.ThrowsAsync<BusinessException>(() => BuildService().GradeAsync(dto, Guid.NewGuid()));

        await _manualGradeRepo.DidNotReceive().UpsertAsync(Arg.Any<ManualGrade>());
    }

    [Fact]
    public async Task GradeAsync_ValidScore_UpsertsGradeAndRecomputesResult()
    {
        var attempt = BuildCompletedAttempt(out var openAnswerQuestionId);
        _attemptRepo.GetByIdWithDetailsAsync(attempt.Id).Returns(attempt);
        var examinerId = Guid.NewGuid();
        var dto = new GradeAnswerDto { AttemptId = attempt.Id, QuestionId = openAnswerQuestionId, Score = 5 };

        await BuildService().GradeAsync(dto, examinerId);

        await _manualGradeRepo.Received(1).UpsertAsync(Arg.Is<ManualGrade>(g =>
            g.AttemptId == attempt.Id &&
            g.QuestionId == openAnswerQuestionId &&
            g.Score == 5 &&
            g.GradedByUserId == examinerId));
        await _examService.Received(1).RecomputeResultAsync(attempt.Id);
    }
}
