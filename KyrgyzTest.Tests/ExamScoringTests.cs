using System.Text.Json;
using KyrgyzTest.Application.Interfaces;
using KyrgyzTest.Application.Services;
using Xunit;
using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Enums;
using KyrgyzTest.Core.Interfaces;
using KyrgyzTest.Tests.Helpers;
using NSubstitute;

namespace KyrgyzTest.Tests;

public class ExamScoringTests
{
    private readonly ICandidateRepository _candidateRepo = Substitute.For<ICandidateRepository>();
    private readonly IAttemptRepository _attemptRepo = Substitute.For<IAttemptRepository>();
    private readonly ISectionConfigRepository _sectionConfigRepo = Substitute.For<ISectionConfigRepository>();
    private readonly ITestVariantGeneratorService _generator = Substitute.For<ITestVariantGeneratorService>();
    private readonly ICandidateAnswerRepository _answerRepo = Substitute.For<ICandidateAnswerRepository>();
    private readonly IResultRepository _resultRepo = Substitute.For<IResultRepository>();
    private readonly ICompletedSectionRepository _completedSectionRepo = Substitute.For<ICompletedSectionRepository>();
    private readonly ISectionTimingRepository _sectionTimingRepo = Substitute.For<ISectionTimingRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    private ExamService BuildService() => new(
        _candidateRepo, _attemptRepo, _sectionConfigRepo,
        _generator, _answerRepo, _resultRepo, _completedSectionRepo, _sectionTimingRepo, _unitOfWork);

    private async Task<Result> SubmitWithAnswers(Attempt attempt, List<CandidateAnswer> answers)
    {
        _attemptRepo.GetByIdWithDetailsAsync(attempt.Id).Returns(attempt);
        _answerRepo.GetByAttemptIdAsync(attempt.Id).Returns(answers);
        _resultRepo.GetByAttemptIdAsync(attempt.Id).Returns((Result?)null);
        _candidateRepo.GetByIdAsync(attempt.CandidateId)
            .Returns(TestData.DefaultCandidate(attempt.CandidateId));
        _candidateRepo.UpdateAsync(Arg.Any<Candidate>())
            .Returns(ci => ci.Arg<Candidate>());

        Result? saved = null;
        _resultRepo.TryAddAsync(Arg.Any<Result>()).Returns(ci =>
        {
            saved = ci.Arg<Result>();
            return true;
        });

        await BuildService().SubmitAsync(attempt.Id);
        return saved!;
    }

    // ── уровень ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task NoAnswers_Level_Is_A1()
    {
        var (attempt, _) = TestData.BuildMcqAttempt(grammarCount: 10);
        var result = await SubmitWithAnswers(attempt, TestData.NoAnswers());
        Assert.Equal(LanguageLevel.A1, result.Level);
    }

    [Theory]
    // boundary: < 0.10 → A1;  anything / 10 where result < 0.10
    [InlineData(0, 10, LanguageLevel.A1)]   // 0 %
    // 1/11 ≈ 0.09 < 0.10 → A1
    // boundary: 0.10 → A2
    [InlineData(1, 10, LanguageLevel.A2)]   // 10 %
    [InlineData(3, 10, LanguageLevel.A2)]   // 30 %
    // boundary: 0.35 → B1
    [InlineData(4, 10, LanguageLevel.B1)]   // 40 %
    [InlineData(5, 10, LanguageLevel.B1)]   // 50 %
    // boundary: 0.60 → B2
    [InlineData(6, 10, LanguageLevel.B2)]   // 60 %
    [InlineData(10, 10, LanguageLevel.B2)]  // 100 %
    public async Task Level_Matches_Percentage(int correct, int total, LanguageLevel expected)
    {
        var (attempt, correctOptionIds) = TestData.BuildMcqAttempt(grammarCount: total);
        var allAnswers = TestData.AllCorrectAnswers(attempt, correctOptionIds);
        var answers = allAnswers.Take(correct).ToList(); // answer only `correct` questions

        var result = await SubmitWithAnswers(attempt, answers);
        Assert.Equal(expected, result.Level);
    }

    // ── очки по секциям ───────────────────────────────────────────────────────

    [Fact]
    public async Task Scores_AreTracked_PerSection()
    {
        var (attempt, correctOptionIds) = TestData.BuildMcqAttempt(
            grammarCount: 3, listeningCount: 2, readingCount: 2, writingCount: 3);
        var answers = TestData.AllCorrectAnswers(attempt, correctOptionIds);

        var result = await SubmitWithAnswers(attempt, answers);

        Assert.Equal(3, result.GrammarScore);
        Assert.Equal(2, result.ListeningScore);
        Assert.Equal(2, result.ReadingScore);
        Assert.Equal(3, result.WritingScore);
        Assert.Equal(10, result.TotalScore);
    }

    [Fact]
    public async Task PartialAnswers_OnlyCorrectOnesCount()
    {
        var (attempt, correctOptionIds) = TestData.BuildMcqAttempt(grammarCount: 4);
        var allAnswers = TestData.AllCorrectAnswers(attempt, correctOptionIds);

        // answer 3 correctly, 1 with wrong option
        var original = allAnswers[3];
        var wrongAnswer = new CandidateAnswer
        {
            Id = Guid.NewGuid(),
            AttemptId = original.AttemptId,
            QuestionId = original.QuestionId,
            SelectedOptionId = Guid.NewGuid() // random → not correct
        };
        var answers = allAnswers.Take(3).Append(wrongAnswer).ToList();

        var result = await SubmitWithAnswers(attempt, answers);
        Assert.Equal(3, result.GrammarScore);
    }

    // ── типы вопросов ─────────────────────────────────────────────────────────

    [Fact]
    public async Task OrderedAnswer_CorrectSequence_CountsAsCorrect()
    {
        var questionId = Guid.NewGuid();
        var opt1 = new AnswerOption { Id = Guid.NewGuid(), QuestionId = questionId, IsCorrect = true, OrderIndex = 0 };
        var opt2 = new AnswerOption { Id = Guid.NewGuid(), QuestionId = questionId, IsCorrect = true, OrderIndex = 1 };
        var question = new Question
        {
            Id = questionId,
            Section = SectionType.Grammar,
            Level = LanguageLevel.A1,
            Type = QuestionType.WordOrder,
            Content = "Order me",
            AnswerOptions = new List<AnswerOption> { opt1, opt2 }
        };

        var attempt = BuildAttemptWithSingleQuestion(question);
        var correctJson = JsonSerializer.Serialize(new[] { opt1.Id, opt2.Id });
        var answers = new List<CandidateAnswer>
        {
            new() { Id = Guid.NewGuid(), AttemptId = attempt.Id, QuestionId = questionId, OrderedAnswer = correctJson }
        };

        var result = await SubmitWithAnswers(attempt, answers);
        Assert.Equal(1, result.GrammarScore);
    }

    [Fact]
    public async Task OrderedAnswer_WrongSequence_NotCounted()
    {
        var questionId = Guid.NewGuid();
        var opt1 = new AnswerOption { Id = Guid.NewGuid(), QuestionId = questionId, IsCorrect = true, OrderIndex = 0 };
        var opt2 = new AnswerOption { Id = Guid.NewGuid(), QuestionId = questionId, IsCorrect = true, OrderIndex = 1 };
        var question = new Question
        {
            Id = questionId,
            Section = SectionType.Grammar,
            Level = LanguageLevel.A1,
            Type = QuestionType.WordOrder,
            Content = "Order me",
            AnswerOptions = new List<AnswerOption> { opt1, opt2 }
        };

        var attempt = BuildAttemptWithSingleQuestion(question);
        var wrongOrderJson = JsonSerializer.Serialize(new[] { opt2.Id, opt1.Id }); // reversed
        var answers = new List<CandidateAnswer>
        {
            new() { Id = Guid.NewGuid(), AttemptId = attempt.Id, QuestionId = questionId, OrderedAnswer = wrongOrderJson }
        };

        var result = await SubmitWithAnswers(attempt, answers);
        Assert.Equal(0, result.GrammarScore);
    }

    [Fact]
    public async Task OrderedAnswer_InvalidJson_NotCounted()
    {
        var questionId = Guid.NewGuid();
        var question = new Question
        {
            Id = questionId,
            Section = SectionType.Grammar,
            Level = LanguageLevel.A1,
            Type = QuestionType.WordOrder,
            Content = "Order me",
            AnswerOptions = new List<AnswerOption>
            {
                new() { Id = Guid.NewGuid(), QuestionId = questionId, IsCorrect = true, OrderIndex = 0 }
            }
        };

        var attempt = BuildAttemptWithSingleQuestion(question);
        var answers = new List<CandidateAnswer>
        {
            new() { Id = Guid.NewGuid(), AttemptId = attempt.Id, QuestionId = questionId, OrderedAnswer = "NOT_JSON" }
        };

        var result = await SubmitWithAnswers(attempt, answers);
        Assert.Equal(0, result.GrammarScore);
    }

    // ── хелпер ───────────────────────────────────────────────────────────────

    private static Attempt BuildAttemptWithSingleQuestion(Question question)
    {
        var tvq = new TestVariantQuestion
        {
            Id = Guid.NewGuid(),
            QuestionId = question.Id,
            Question = question,
            OrderIndex = 0
        };
        var variant = new TestVariant { Id = Guid.NewGuid(), Number = 1, Questions = new List<TestVariantQuestion> { tvq } };
        return new Attempt
        {
            Id = Guid.NewGuid(),
            CandidateId = Guid.NewGuid(),
            TestVariantId = variant.Id,
            TestVariant = variant,
            Status = AttemptStatus.InProgress
        };
    }
}
