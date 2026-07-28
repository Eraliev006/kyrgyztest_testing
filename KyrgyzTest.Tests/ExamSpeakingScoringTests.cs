using KyrgyzTest.Application.Interfaces;
using KyrgyzTest.Application.Services;
using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Enums;
using KyrgyzTest.Core.Interfaces;
using KyrgyzTest.Tests.Helpers;
using NSubstitute;
using Xunit;

namespace KyrgyzTest.Tests;

public class ExamSpeakingScoringTests
{
    private readonly ICandidateRepository _candidateRepo = Substitute.For<ICandidateRepository>();
    private readonly IAttemptRepository _attemptRepo = Substitute.For<IAttemptRepository>();
    private readonly ISectionConfigRepository _sectionConfigRepo = Substitute.For<ISectionConfigRepository>();
    private readonly ITestVariantRepository _variantRepo = Substitute.For<ITestVariantRepository>();
    private readonly ICandidateAnswerRepository _answerRepo = Substitute.For<ICandidateAnswerRepository>();
    private readonly IResultRepository _resultRepo = Substitute.For<IResultRepository>();
    private readonly ICompletedSectionRepository _completedSectionRepo = Substitute.For<ICompletedSectionRepository>();
    private readonly ISectionTimingRepository _sectionTimingRepo = Substitute.For<ISectionTimingRepository>();
    private readonly IManualGradeRepository _manualGradeRepo = Substitute.For<IManualGradeRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    private ExamService BuildService() => new(
        _candidateRepo, _attemptRepo, _sectionConfigRepo,
        _variantRepo, _answerRepo, _resultRepo, _completedSectionRepo, _sectionTimingRepo, _manualGradeRepo, _unitOfWork);

    private static Attempt BuildOpenAnswerAttempt(out Guid questionId)
    {
        questionId = Guid.NewGuid();
        var question = new Question
        {
            Id = questionId,
            Section = SectionType.Speaking,
            Level = LanguageLevel.A1,
            Type = QuestionType.OpenAnswer,
            Content = "Describe your day",
            AnswerOptions = new List<AnswerOption>(),
        };
        var variant = new TestVariant
        {
            Id = Guid.NewGuid(),
            Number = 1,
            Questions = new List<TestVariantQuestion>
            {
                new() { Id = Guid.NewGuid(), QuestionId = questionId, Question = question, OrderIndex = 0 },
            },
        };
        return new Attempt
        {
            Id = Guid.NewGuid(),
            CandidateId = Guid.NewGuid(),
            TestVariantId = variant.Id,
            TestVariant = variant,
            Status = AttemptStatus.InProgress,
        };
    }

    [Fact]
    public async Task Submit_UngradedOpenAnswer_ContributesZero_ToSpeakingScore()
    {
        var attempt = BuildOpenAnswerAttempt(out var questionId);
        _attemptRepo.GetByIdWithDetailsAsync(attempt.Id).Returns(attempt);
        _answerRepo.GetByAttemptIdAsync(attempt.Id).Returns(new List<CandidateAnswer>
        {
            new() { Id = Guid.NewGuid(), AttemptId = attempt.Id, QuestionId = questionId, AudioAnswerUrl = "/uploads/a.webm" },
        });
        _manualGradeRepo.GetByAttemptIdAsync(attempt.Id).Returns(new List<ManualGrade>());
        _resultRepo.GetByAttemptIdAsync(attempt.Id).Returns((Result?)null);
        _candidateRepo.GetByIdAsync(attempt.CandidateId).Returns(TestData.DefaultCandidate(attempt.CandidateId));
        _candidateRepo.UpdateAsync(Arg.Any<Candidate>()).Returns(ci => ci.Arg<Candidate>());

        Result? saved = null;
        _resultRepo.TryAddAsync(Arg.Any<Result>()).Returns(ci =>
        {
            saved = ci.Arg<Result>();
            return true;
        });

        await BuildService().SubmitAsync(attempt.Id);

        Assert.Equal(0, saved!.SpeakingScore);
        Assert.Equal(LanguageLevel.A1, saved.Level);
    }

    [Fact]
    public async Task RecomputeResultAsync_FoldsInManualGrade_AfterExaminerGrades()
    {
        var attempt = BuildOpenAnswerAttempt(out var questionId);
        var existingResult = new Result
        {
            Id = Guid.NewGuid(),
            AttemptId = attempt.Id,
            CandidateId = attempt.CandidateId,
            Level = LanguageLevel.A1,
            SpeakingScore = 0,
            TotalScore = 0,
            CreatedAt = DateTime.UtcNow,
        };

        _attemptRepo.GetByIdWithDetailsAsync(attempt.Id).Returns(attempt);
        _resultRepo.GetByAttemptIdAsync(attempt.Id).Returns(existingResult);
        _answerRepo.GetByAttemptIdAsync(attempt.Id).Returns(new List<CandidateAnswer>());
        _manualGradeRepo.GetByAttemptIdAsync(attempt.Id).Returns(new List<ManualGrade>
        {
            new() { Id = Guid.NewGuid(), AttemptId = attempt.Id, QuestionId = questionId, Score = 4, GradedByUserId = Guid.NewGuid(), GradedAt = DateTime.UtcNow },
        });

        await BuildService().RecomputeResultAsync(attempt.Id);

        await _resultRepo.Received(1).UpdateAsync(Arg.Is<Result>(r => r.SpeakingScore == 4 && r.TotalScore == 4));
        // Single OpenAnswer question graded 4/5 -> 80% -> B2.
        await _resultRepo.Received(1).UpdateAsync(Arg.Is<Result>(r => r.Level == LanguageLevel.B2));
    }

    [Fact]
    public async Task Submit_MixedMcqAndOpenAnswer_WeightsOpenAnswerAsFivePoints()
    {
        var speakingQuestionId = Guid.NewGuid();
        var speakingQuestion = new Question
        {
            Id = speakingQuestionId,
            Section = SectionType.Speaking,
            Level = LanguageLevel.A1,
            Type = QuestionType.OpenAnswer,
            Content = "Speaking prompt",
            AnswerOptions = new List<AnswerOption>(),
        };

        var (mcqAttempt, correctOptionIds) = TestData.BuildMcqAttempt(grammarCount: 1);
        mcqAttempt.TestVariant.Questions.Add(new TestVariantQuestion
        {
            Id = Guid.NewGuid(),
            QuestionId = speakingQuestionId,
            Question = speakingQuestion,
            OrderIndex = 1,
        });

        _attemptRepo.GetByIdWithDetailsAsync(mcqAttempt.Id).Returns(mcqAttempt);
        var mcqQuestionId = mcqAttempt.TestVariant.Questions.First(q => q.QuestionId != speakingQuestionId).QuestionId;
        _answerRepo.GetByAttemptIdAsync(mcqAttempt.Id).Returns(new List<CandidateAnswer>
        {
            new() { Id = Guid.NewGuid(), AttemptId = mcqAttempt.Id, QuestionId = mcqQuestionId, SelectedOptionId = correctOptionIds[mcqQuestionId] },
        });
        // Speaking question graded 5/5 (max) — combined with 1 correct MCQ (1/1), total = 6 out of max 6 (1 + 5) = 100%.
        _manualGradeRepo.GetByAttemptIdAsync(mcqAttempt.Id).Returns(new List<ManualGrade>
        {
            new() { Id = Guid.NewGuid(), AttemptId = mcqAttempt.Id, QuestionId = speakingQuestionId, Score = 5, GradedByUserId = Guid.NewGuid(), GradedAt = DateTime.UtcNow },
        });
        _resultRepo.GetByAttemptIdAsync(mcqAttempt.Id).Returns((Result?)null);
        _candidateRepo.GetByIdAsync(mcqAttempt.CandidateId).Returns(TestData.DefaultCandidate(mcqAttempt.CandidateId));
        _candidateRepo.UpdateAsync(Arg.Any<Candidate>()).Returns(ci => ci.Arg<Candidate>());

        Result? saved = null;
        _resultRepo.TryAddAsync(Arg.Any<Result>()).Returns(ci =>
        {
            saved = ci.Arg<Result>();
            return true;
        });

        await BuildService().SubmitAsync(mcqAttempt.Id);

        Assert.Equal(1, saved!.GrammarScore);
        Assert.Equal(5, saved.SpeakingScore);
        Assert.Equal(6, saved.TotalScore);
        Assert.Equal(LanguageLevel.B2, saved.Level);
    }
}
