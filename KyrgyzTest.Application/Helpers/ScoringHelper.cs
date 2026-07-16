using System.Text.Json;
using KyrgyzTest.Core.Entities;

namespace KyrgyzTest.Application.Helpers;

internal static class ScoringHelper
{
    public static bool IsCorrectMcq(Question question, CandidateAnswer answer)
    {
        if (answer.SelectedOptionId == null) return false;
        var correctOption = question.AnswerOptions.FirstOrDefault(o => o.IsCorrect);
        return correctOption?.Id == answer.SelectedOptionId;
    }

    public static bool IsCorrectOrdered(Question question, CandidateAnswer answer)
    {
        if (string.IsNullOrEmpty(answer.OrderedAnswer)) return false;

        Guid[] submitted;
        try
        {
            submitted = JsonSerializer.Deserialize<Guid[]>(answer.OrderedAnswer) ?? [];
        }
        catch
        {
            return false;
        }

        var correctOrder = question.AnswerOptions
            .Where(o => o.IsCorrect)
            .OrderBy(o => o.OrderIndex)
            .Select(o => o.Id)
            .ToArray();

        return submitted.SequenceEqual(correctOrder);
    }

    // Overload used by ResultService where correctOrder is already pre-computed.
    public static bool IsCorrectOrdered(string? orderedAnswer, Guid[] correctOrder)
    {
        if (string.IsNullOrEmpty(orderedAnswer)) return false;
        try
        {
            var submitted = JsonSerializer.Deserialize<Guid[]>(orderedAnswer) ?? [];
            return submitted.SequenceEqual(correctOrder);
        }
        catch { return false; }
    }
}
