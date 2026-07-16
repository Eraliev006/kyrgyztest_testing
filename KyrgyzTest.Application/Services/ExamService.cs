using KyrgyzTest.Application.DTOs;
using KyrgyzTest.Application.Helpers;
using KyrgyzTest.Application.Interfaces;
using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Enums;
using KyrgyzTest.Core.Exceptions;
using KyrgyzTest.Core.Interfaces;

namespace KyrgyzTest.Application.Services;

public class ExamService : IExamService
{
    private readonly ICandidateRepository _candidateRepository;
    private readonly IAttemptRepository _attemptRepository;
    private readonly ISectionConfigRepository _sectionConfigRepository;
    private readonly ITestVariantGeneratorService _generatorService;
    private readonly ICandidateAnswerRepository _candidateAnswerRepository;
    private readonly IResultRepository _resultRepository;
    private readonly ICompletedSectionRepository _completedSectionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ExamService(
        ICandidateRepository candidateRepository,
        IAttemptRepository attemptRepository,
        ISectionConfigRepository sectionConfigRepository,
        ITestVariantGeneratorService generatorService,
        ICandidateAnswerRepository candidateAnswerRepository,
        IResultRepository resultRepository,
        ICompletedSectionRepository completedSectionRepository,
        IUnitOfWork unitOfWork)
    {
        _candidateRepository = candidateRepository;
        _attemptRepository = attemptRepository;
        _sectionConfigRepository = sectionConfigRepository;
        _generatorService = generatorService;
        _candidateAnswerRepository = candidateAnswerRepository;
        _resultRepository = resultRepository;
        _completedSectionRepository = completedSectionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<StartExamResultDto> StartAsync(Guid candidateId)
    {
        var candidate = await _candidateRepository.GetByIdAsync(candidateId)
            ?? throw new NotFoundException($"Кандидат {candidateId} не найден");

        if (!candidate.IsAllowed)
            throw new BusinessException("Кандидату не разрешён доступ к экзамену");

        var active = await _attemptRepository.GetActiveByCandidate(candidateId);
        if (active != null)
        {
            var existing = await _attemptRepository.GetByIdWithDetailsAsync(active.Id)
                ?? throw new NotFoundException($"Попытка {active.Id} не найдена");
            return await BuildStartResultAsync(existing.Id, existing.TestVariant.Questions);
        }

        var testVariant = await _generatorService.GenerateAsync();

        var attempt = new Attempt
        {
            Id = Guid.NewGuid(),
            CandidateId = candidateId,
            TestVariantId = testVariant.Id,
            Status = AttemptStatus.InProgress,
            StartedAt = DateTime.UtcNow
        };
        await _attemptRepository.AddAsync(attempt);

        return await BuildStartResultAsync(attempt.Id, testVariant.Questions);
    }

    private async Task<StartExamResultDto> BuildStartResultAsync(Guid attemptId, IEnumerable<TestVariantQuestion> variantQuestions)
    {
        var configs = await _sectionConfigRepository.GetAll();
        var configBySection = configs.ToDictionary(c => c.Section);

        var completedSections = await _completedSectionRepository.GetByAttemptIdAsync(attemptId);
        var completedSet = completedSections.Select(cs => cs.Section).ToHashSet();

        var sections = variantQuestions
            .GroupBy(vq => vq.Question.Section)
            .Select(g =>
            {
                var sectionType = g.Key;
                var timeLimit = configBySection.TryGetValue(sectionType, out var cfg) ? cfg.TimeLimitMinutes : 0;
                return new ExamSectionDto { Section = sectionType, TimeLimitMinutes = timeLimit, IsCompleted = completedSet.Contains(sectionType) };
            }).ToList();

        return new StartExamResultDto { AttemptId = attemptId, Sections = sections };
    }

    public async Task<ExamSectionDto> StartSectionAsync(Guid attemptId, SectionType section)
    {
        var attempt = await _attemptRepository.GetByIdWithDetailsAsync(attemptId)
            ?? throw new NotFoundException($"Попытка {attemptId} не найдена");

        if (attempt.Status != AttemptStatus.InProgress)
            throw new BusinessException("Попытка не активна");

        var completedSections = await _completedSectionRepository.GetByAttemptIdAsync(attemptId);
        if (completedSections.Any(cs => cs.Section == section))
            throw new BusinessException($"Секция {section} уже завершена");

        var configs = await _sectionConfigRepository.GetAll();
        var config = configs.FirstOrDefault(c => c.Section == section);

        var questions = attempt.TestVariant.Questions
            .Where(vq => vq.Question.Section == section)
            .OrderBy(vq => vq.OrderIndex)
            .Select(vq => new ExamQuestionDto
            {
                Id = vq.Question.Id,
                Content = vq.Question.Content,
                Type = vq.Question.Type,
                AnswerOptions = vq.Question.AnswerOptions.Select(ao => new ExamAnswerOptionDto
                {
                    Id = ao.Id,
                    Content = ao.Content
                }).ToList()
            }).ToList();

        return new ExamSectionDto
        {
            Section = section,
            TimeLimitMinutes = config?.TimeLimitMinutes ?? 0,
            Questions = questions
        };
    }

    public async Task<SubmitSectionResultDto> SubmitSectionAsync(Guid attemptId, SectionType section)
    {
        var attempt = await _attemptRepository.GetByIdWithDetailsAsync(attemptId)
            ?? throw new NotFoundException($"Попытка {attemptId} не найдена");

        if (attempt.Status != AttemptStatus.InProgress)
            throw new BusinessException("Попытка не активна");

        var completedSections = await _completedSectionRepository.GetByAttemptIdAsync(attemptId);
        if (completedSections.Any(cs => cs.Section == section))
            throw new BusinessException($"Секция {section} уже завершена");

        var inserted = await _completedSectionRepository.TryAddAsync(new CompletedSection
        {
            Id = Guid.NewGuid(),
            AttemptId = attemptId,
            Section = section,
            CompletedAt = DateTime.UtcNow
        });
        if (!inserted)
            throw new BusinessException($"Секция {section} уже завершена");

        var totalSections = attempt.TestVariant.Questions
            .Select(tvq => tvq.Question.Section)
            .Distinct()
            .Count();

        if (completedSections.Count + 1 >= totalSections)
        {
            var result = await SubmitAsync(attemptId);
            return new SubmitSectionResultDto { IsExamCompleted = true, Result = result };
        }

        return new SubmitSectionResultDto { IsExamCompleted = false };
    }

    public async Task SaveAnswerAsync(SaveAnswerDto dto)
    {
        var attempt = await _attemptRepository.GetByIdWithDetailsAsync(dto.AttemptId)
            ?? throw new NotFoundException($"Попытка {dto.AttemptId} не найдена");

        if (attempt.Status != AttemptStatus.InProgress)
            throw new BusinessException("Попытка не активна");

        var answer = new CandidateAnswer
        {
            Id = Guid.NewGuid(),
            AttemptId = dto.AttemptId,
            QuestionId = dto.QuestionId,
            SelectedOptionId = dto.SelectedOptionId,
            OrderedAnswer = dto.OrderedAnswer
        };

        await _candidateAnswerRepository.UpsertAsync(answer);
    }

    public async Task<ResultResponseDto> SubmitAsync(Guid attemptId)
    {
        var attempt = await _attemptRepository.GetByIdWithDetailsAsync(attemptId)
            ?? throw new NotFoundException($"Попытка {attemptId} не найдена");

        // Idempotency: if Result already exists, finish any partial cleanup and return it.
        var existingResult = await _resultRepository.GetByAttemptIdAsync(attemptId);
        if (existingResult != null)
            return await FinishCleanupAndMapAsync(attempt, existingResult);

        if (attempt.Status != AttemptStatus.InProgress)
            throw new BusinessException("Попытка не активна");

        var answers = await _candidateAnswerRepository.GetByAttemptIdAsync(attemptId);
        var answerMap = answers.ToDictionary(a => a.QuestionId);

        int grammarScore = 0, listeningScore = 0, readingScore = 0, writingScore = 0;
        int totalQuestions = attempt.TestVariant.Questions.Count;

        foreach (var tvq in attempt.TestVariant.Questions)
        {
            var question = tvq.Question;
            if (!answerMap.TryGetValue(question.Id, out var candidateAnswer))
                continue;

            bool isCorrect = question.Type == QuestionType.MCQ
                ? ScoringHelper.IsCorrectMcq(question, candidateAnswer)
                : ScoringHelper.IsCorrectOrdered(question, candidateAnswer);

            if (!isCorrect) continue;

            switch (question.Section)
            {
                case SectionType.Grammar: grammarScore++; break;
                case SectionType.Listening: listeningScore++; break;
                case SectionType.Reading: readingScore++; break;
                case SectionType.Writing: writingScore++; break;
            }
        }

        int totalScore = grammarScore + listeningScore + readingScore + writingScore;
        double percentage = totalQuestions > 0 ? (double)totalScore / totalQuestions : 0;

        var level = percentage switch
        {
            < 0.10 => LanguageLevel.A1,
            < 0.35 => LanguageLevel.A2,
            < 0.60 => LanguageLevel.B1,
            _ => LanguageLevel.B2
        };

        var result = new Result
        {
            Id = Guid.NewGuid(),
            AttemptId = attemptId,
            CandidateId = attempt.CandidateId,
            Level = level,
            GrammarScore = grammarScore,
            ListeningScore = listeningScore,
            ReadingScore = readingScore,
            WritingScore = writingScore,
            TotalScore = totalScore,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var inserted = await _resultRepository.TryAddAsync(result);
            if (!inserted)
            {
                // Concurrent race: another request inserted Result first.
                await _unitOfWork.RollbackAsync();
                var raceResult = await _resultRepository.GetByAttemptIdAsync(attemptId)
                    ?? throw new BusinessException("Попытка уже завершается");
                return await FinishCleanupAndMapAsync(attempt, raceResult);
            }

            attempt.Status = AttemptStatus.Completed;
            attempt.SubmittedAt = DateTime.UtcNow;
            await _attemptRepository.UpdateAsync(attempt);

            var candidate = await _candidateRepository.GetByIdAsync(attempt.CandidateId)
                ?? throw new NotFoundException($"Кандидат {attempt.CandidateId} не найден");
            candidate.IsAllowed = false;
            candidate.Photo = null;
            await _candidateRepository.UpdateAsync(candidate);

            await _unitOfWork.CommitAsync();
            return MapResult(result);
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    private async Task<ResultResponseDto> FinishCleanupAndMapAsync(Attempt attempt, Result result)
    {
        if (attempt.Status == AttemptStatus.InProgress)
        {
            attempt.Status = AttemptStatus.Completed;
            attempt.SubmittedAt ??= DateTime.UtcNow;
            await _attemptRepository.UpdateAsync(attempt);
        }

        var candidate = await _candidateRepository.GetByIdAsync(attempt.CandidateId);
        if (candidate is { IsAllowed: true })
        {
            candidate.IsAllowed = false;
            candidate.Photo = null;
            await _candidateRepository.UpdateAsync(candidate);
        }

        return MapResult(result);
    }

    private static ResultResponseDto MapResult(Result r) => new()
    {
        Id = r.Id,
        AttemptId = r.AttemptId,
        CandidateId = r.CandidateId,
        Level = r.Level,
        GrammarScore = r.GrammarScore,
        ListeningScore = r.ListeningScore,
        ReadingScore = r.ReadingScore,
        WritingScore = r.WritingScore,
        TotalScore = r.TotalScore,
        CreatedAt = r.CreatedAt
    };

    public async Task<bool> HasActiveAttemptAsync(Guid candidateId)
    {
        var active = await _attemptRepository.GetActiveByCandidate(candidateId);
        return active != null;
    }
}
