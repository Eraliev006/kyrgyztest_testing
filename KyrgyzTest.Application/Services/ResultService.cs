using System.Text.Json;
using KyrgyzTest.Application.DTOs;
using KyrgyzTest.Application.Interfaces;
using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Enums;
using KyrgyzTest.Core.Exceptions;
using KyrgyzTest.Core.Interfaces;

namespace KyrgyzTest.Application.Services;

public class ResultService : IResultService
{
    private readonly IResultRepository _repository;
    private readonly IAttemptRepository _attemptRepository;
    private readonly ICandidateAnswerRepository _candidateAnswerRepository;

    public ResultService(
        IResultRepository repository,
        IAttemptRepository attemptRepository,
        ICandidateAnswerRepository candidateAnswerRepository)
    {
        _repository = repository;
        _attemptRepository = attemptRepository;
        _candidateAnswerRepository = candidateAnswerRepository;
    }

    public async Task<List<ResultWithCandidateDto>> GetAllAsync(Guid? organizationId, LanguageLevel? level, DateTime? dateFrom, DateTime? dateTo)
    {
        var results = await _repository.GetFilteredAsync(organizationId, level, dateFrom, dateTo);
        return results.Select(Map).ToList();
    }

    public async Task<ResultWithCandidateDto> GetByCandidateIdAsync(Guid candidateId)
    {
        var result = await _repository.GetLatestByCandidateIdAsync(candidateId)
            ?? throw new NotFoundException("Результат не найден");
        return Map(result);
    }

    public async Task<OrgStatsDto> GetStatsAsync(Guid? organizationId, DateTime? dateFrom, DateTime? dateTo)
    {
        var results = await _repository.GetFilteredAsync(organizationId, null, dateFrom, dateTo);

        var stats = new OrgStatsDto
        {
            TotalCount = results.Count,
            AverageScore = results.Count > 0 ? results.Average(r => r.TotalScore) : 0,
            ByLevel = new Dictionary<string, int>
            {
                ["a1"] = results.Count(r => r.Level == LanguageLevel.A1),
                ["a2"] = results.Count(r => r.Level == LanguageLevel.A2),
                ["b1"] = results.Count(r => r.Level == LanguageLevel.B1),
                ["b2"] = results.Count(r => r.Level == LanguageLevel.B2)
            }
        };

        return stats;
    }

    public async Task<AttemptDetailDto> GetAttemptDetailsAsync(Guid attemptId)
    {
        var attempt = await _attemptRepository.GetByIdWithDetailsAsync(attemptId)
            ?? throw new NotFoundException("Попытка не найдена");

        var answers = await _candidateAnswerRepository.GetByAttemptIdAsync(attemptId);
        var answerMap = answers.ToDictionary(a => a.QuestionId);

        var result = new AttemptDetailDto
        {
            AttemptId = attempt.Id,
            CandidateId = attempt.CandidateId,
        };

        foreach (var tvq in attempt.TestVariant.Questions)
        {
            var question = tvq.Question;
            answerMap.TryGetValue(question.Id, out var candidateAnswer);

            var correctOption = question.AnswerOptions.FirstOrDefault(o => o.IsCorrect);
            var correctOrder = question.AnswerOptions
                .Where(o => o.IsCorrect)
                .OrderBy(o => o.OrderIndex)
                .Select(o => o.Id)
                .ToArray();

            bool isCorrect = false;
            if (candidateAnswer != null)
            {
                isCorrect = question.Type == QuestionType.MCQ
                    ? correctOption?.Id == candidateAnswer.SelectedOptionId
                    : IsCorrectOrdered(candidateAnswer.OrderedAnswer, correctOrder);
            }

            result.Answers.Add(new AttemptAnswerDetailDto
            {
                QuestionId = question.Id,
                Content = question.Content,
                Section = question.Section,
                Level = question.Level,
                Type = question.Type,
                SelectedOptionId = candidateAnswer?.SelectedOptionId,
                OrderedAnswer = candidateAnswer?.OrderedAnswer,
                IsCorrect = isCorrect,
                CorrectOptionId = question.Type == QuestionType.MCQ ? correctOption?.Id : null,
                CorrectOrder = question.Type != QuestionType.MCQ ? correctOrder : null,
            });
        }

        return result;
    }

    private static bool IsCorrectOrdered(string? orderedAnswer, Guid[] correctOrder)
    {
        if (string.IsNullOrEmpty(orderedAnswer)) return false;
        try
        {
            var submitted = JsonSerializer.Deserialize<Guid[]>(orderedAnswer) ?? [];
            return submitted.SequenceEqual(correctOrder);
        }
        catch { return false; }
    }

    private static ResultWithCandidateDto Map(Result r) => new()
    {
        Id = r.Id,
        AttemptId = r.AttemptId,
        CandidateId = r.CandidateId,
        CandidateFullName = r.Candidate.FullName,
        CandidateInn = r.Candidate.Inn,
        OrganizationId = r.Candidate.OrganizationId,
        OrganizationShortName = r.Candidate.Organization?.ShortName,
        Level = r.Level,
        GrammarScore = r.GrammarScore,
        ListeningScore = r.ListeningScore,
        ReadingScore = r.ReadingScore,
        WritingScore = r.WritingScore,
        TotalScore = r.TotalScore,
        CreatedAt = r.CreatedAt
    };
}
