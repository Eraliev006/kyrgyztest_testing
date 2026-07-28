using KyrgyzTest.Application.DTOs;
using KyrgyzTest.Application.Interfaces;
using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Enums;
using KyrgyzTest.Core.Exceptions;
using KyrgyzTest.Core.Interfaces;

namespace KyrgyzTest.Application.Services;

public class ExaminerService : IExaminerService
{
    private readonly IAttemptRepository _attemptRepository;
    private readonly ICandidateAnswerRepository _candidateAnswerRepository;
    private readonly IManualGradeRepository _manualGradeRepository;
    private readonly IExamService _examService;

    public ExaminerService(
        IAttemptRepository attemptRepository,
        ICandidateAnswerRepository candidateAnswerRepository,
        IManualGradeRepository manualGradeRepository,
        IExamService examService)
    {
        _attemptRepository = attemptRepository;
        _candidateAnswerRepository = candidateAnswerRepository;
        _manualGradeRepository = manualGradeRepository;
        _examService = examService;
    }

    public async Task<List<ExaminerQueueItemDto>> GetQueueAsync()
    {
        var attempts = await _attemptRepository.GetCompletedWithDetailsAsync();
        var queue = new List<ExaminerQueueItemDto>();

        foreach (var attempt in attempts)
        {
            var openAnswerQuestionIds = attempt.TestVariant.Questions
                .Where(tvq => tvq.Question.Type == QuestionType.OpenAnswer)
                .Select(tvq => tvq.QuestionId)
                .ToList();
            if (openAnswerQuestionIds.Count == 0) continue;

            var grades = await _manualGradeRepository.GetByAttemptIdAsync(attempt.Id);
            var gradedIds = grades.Select(g => g.QuestionId).ToHashSet();
            var ungradedCount = openAnswerQuestionIds.Count(id => !gradedIds.Contains(id));
            if (ungradedCount == 0) continue;

            queue.Add(new ExaminerQueueItemDto
            {
                AttemptId = attempt.Id,
                CandidateId = attempt.CandidateId,
                CandidateFullName = attempt.Candidate.FullName,
                SubmittedAt = attempt.SubmittedAt ?? attempt.StartedAt,
                UngradedCount = ungradedCount
            });
        }

        return queue;
    }

    public async Task<ExaminerReviewDto> GetReviewAsync(Guid attemptId)
    {
        var attempt = await _attemptRepository.GetByIdWithDetailsAsync(attemptId)
            ?? throw new NotFoundException($"Попытка {attemptId} не найдена");

        var answers = await _candidateAnswerRepository.GetByAttemptIdAsync(attemptId);
        var answerMap = answers.ToDictionary(a => a.QuestionId);
        var grades = await _manualGradeRepository.GetByAttemptIdAsync(attemptId);
        var gradeMap = grades.ToDictionary(g => g.QuestionId, g => g.Score);

        var openAnswerQuestions = attempt.TestVariant.Questions
            .Where(tvq => tvq.Question.Type == QuestionType.OpenAnswer)
            .Select(tvq => tvq.Question)
            .ToList();

        return new ExaminerReviewDto
        {
            AttemptId = attempt.Id,
            CandidateFullName = attempt.Candidate.FullName,
            Answers = openAnswerQuestions.Select(q => new ExaminerAnswerDto
            {
                QuestionId = q.Id,
                Content = q.Content,
                AudioAnswerUrl = answerMap.TryGetValue(q.Id, out var a) ? a.AudioAnswerUrl : null,
                Score = gradeMap.TryGetValue(q.Id, out var s) ? s : (int?)null
            }).ToList()
        };
    }

    public async Task GradeAsync(GradeAnswerDto dto, Guid examinerUserId)
    {
        if (dto.Score < 0 || dto.Score > 5)
            throw new ValidationException("Оценка должна быть от 0 до 5");

        var attempt = await _attemptRepository.GetByIdWithDetailsAsync(dto.AttemptId)
            ?? throw new NotFoundException($"Попытка {dto.AttemptId} не найдена");

        var question = attempt.TestVariant.Questions
            .Select(tvq => tvq.Question)
            .FirstOrDefault(q => q.Id == dto.QuestionId)
            ?? throw new NotFoundException($"Вопрос {dto.QuestionId} не найден в этой попытке");

        if (question.Type != QuestionType.OpenAnswer)
            throw new BusinessException("Вручную можно оценивать только вопросы типа OpenAnswer");

        await _manualGradeRepository.UpsertAsync(new ManualGrade
        {
            Id = Guid.NewGuid(),
            AttemptId = dto.AttemptId,
            QuestionId = dto.QuestionId,
            Score = dto.Score,
            GradedByUserId = examinerUserId,
            GradedAt = DateTime.UtcNow
        });

        await _examService.RecomputeResultAsync(dto.AttemptId);
    }
}
