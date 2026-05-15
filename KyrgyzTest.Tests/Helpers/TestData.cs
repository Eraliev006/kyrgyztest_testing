using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Enums;

namespace KyrgyzTest.Tests.Helpers;

internal static class TestData
{
    /// <summary>
    /// Builds an InProgress attempt with MCQ questions.
    /// Each question has one correct AnswerOption.
    /// Returns both the attempt and a dictionary of correct OptionId per QuestionId.
    /// </summary>
    internal static (Attempt attempt, Dictionary<Guid, Guid> correctOptionIds) BuildMcqAttempt(
        int grammarCount,
        int listeningCount = 0,
        int readingCount = 0,
        int writingCount = 0)
    {
        var questions = new List<TestVariantQuestion>();
        var correctOptionIds = new Dictionary<Guid, Guid>();
        int order = 0;

        void Add(SectionType section, int count)
        {
            for (int i = 0; i < count; i++)
            {
                var questionId = Guid.NewGuid();
                var correctOptionId = Guid.NewGuid();
                var question = new Question
                {
                    Id = questionId,
                    Section = section,
                    Level = LanguageLevel.A1,
                    Type = QuestionType.MCQ,
                    Content = $"{section} Q{i}",
                    AnswerOptions = new List<AnswerOption>
                    {
                        new() { Id = correctOptionId, QuestionId = questionId, IsCorrect = true, OrderIndex = 0 },
                        new() { Id = Guid.NewGuid(), QuestionId = questionId, IsCorrect = false, OrderIndex = 1 }
                    }
                };
                correctOptionIds[questionId] = correctOptionId;
                questions.Add(new TestVariantQuestion
                {
                    Id = Guid.NewGuid(),
                    QuestionId = questionId,
                    Question = question,
                    OrderIndex = order++
                });
            }
        }

        Add(SectionType.Grammar, grammarCount);
        Add(SectionType.Listening, listeningCount);
        Add(SectionType.Reading, readingCount);
        Add(SectionType.Writing, writingCount);

        var variant = new TestVariant { Id = Guid.NewGuid(), Number = 1, Questions = questions };
        var candidateId = Guid.NewGuid();
        var attempt = new Attempt
        {
            Id = Guid.NewGuid(),
            CandidateId = candidateId,
            TestVariantId = variant.Id,
            TestVariant = variant,
            Status = AttemptStatus.InProgress
        };
        return (attempt, correctOptionIds);
    }

    internal static List<CandidateAnswer> AllCorrectAnswers(
        Attempt attempt,
        Dictionary<Guid, Guid> correctOptionIds)
        => attempt.TestVariant.Questions
            .Select(tvq => new CandidateAnswer
            {
                Id = Guid.NewGuid(),
                AttemptId = attempt.Id,
                QuestionId = tvq.QuestionId,
                SelectedOptionId = correctOptionIds[tvq.QuestionId]
            })
            .ToList();

    internal static List<CandidateAnswer> NoAnswers() => new();

    internal static Candidate DefaultCandidate(Guid id) => new()
    {
        Id = id,
        FullName = "Test Candidate",
        Inn = "12345678901234",
        AccessCode = "20260515-1234",
        IsAllowed = true
    };
}
