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
    private readonly ITestVariantRepository _testVariantRepository;
    private readonly ICandidateAnswerRepository _candidateAnswerRepository;
    private readonly IResultRepository _resultRepository;
    private readonly ICompletedSectionRepository _completedSectionRepository;
    private readonly ISectionTimingRepository _sectionTimingRepository;
    private readonly IManualGradeRepository _manualGradeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ExamService(
        ICandidateRepository candidateRepository,
        IAttemptRepository attemptRepository,
        ISectionConfigRepository sectionConfigRepository,
        ITestVariantRepository testVariantRepository,
        ICandidateAnswerRepository candidateAnswerRepository,
        IResultRepository resultRepository,
        ICompletedSectionRepository completedSectionRepository,
        ISectionTimingRepository sectionTimingRepository,
        IManualGradeRepository manualGradeRepository,
        IUnitOfWork unitOfWork)
    {
        _candidateRepository = candidateRepository;
        _attemptRepository = attemptRepository;
        _sectionConfigRepository = sectionConfigRepository;
        _testVariantRepository = testVariantRepository;
        _candidateAnswerRepository = candidateAnswerRepository;
        _resultRepository = resultRepository;
        _completedSectionRepository = completedSectionRepository;
        _sectionTimingRepository = sectionTimingRepository;
        _manualGradeRepository = manualGradeRepository;
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

        var testVariant = await _testVariantRepository.GetRandomActiveAsync()
            ?? throw new BusinessException("Нет доступных вариантов теста. Обратитесь к администратору.");

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
        var timeLimitMinutes = config?.TimeLimitMinutes ?? 0;

        var timing = await _sectionTimingRepository.GetOrStartAsync(attemptId, section);

        var existingAnswers = await _candidateAnswerRepository.GetByAttemptIdAsync(attemptId) ?? [];
        var answerByQuestionId = existingAnswers.ToDictionary(a => a.QuestionId);

        var questions = attempt.TestVariant.Questions
            .Where(vq => vq.Question.Section == section)
            .OrderBy(vq => vq.OrderIndex)
            .Select(vq =>
            {
                answerByQuestionId.TryGetValue(vq.Question.Id, out var existing);
                return new ExamQuestionDto
                {
                    Id = vq.Question.Id,
                    Content = vq.Question.Content,
                    Type = vq.Question.Type,
                    AnswerOptions = vq.Question.AnswerOptions.Select(ao => new ExamAnswerOptionDto
                    {
                        Id = ao.Id,
                        Content = ao.Content
                    }).ToList(),
                    SelectedOptionId = existing?.SelectedOptionId,
                    OrderedAnswer = existing?.OrderedAnswer,
                    AudioAnswerUrl = existing?.AudioAnswerUrl
                };
            }).ToList();

        return new ExamSectionDto
        {
            Section = section,
            TimeLimitMinutes = timeLimitMinutes,
            DeadlineUtc = timing.StartedAt.AddMinutes(timeLimitMinutes),
            Questions = questions
        };
    }

    public async Task<SectionStatusDto> GetSectionStatusAsync(Guid attemptId, SectionType section)
    {
        var completedSections = await _completedSectionRepository.GetByAttemptIdAsync(attemptId);
        var isCompleted = completedSections.Any(cs => cs.Section == section);

        var timing = await _sectionTimingRepository.GetByAttemptAndSectionAsync(attemptId, section);
        if (timing == null)
            return new SectionStatusDto { IsCompleted = isCompleted, DeadlineUtc = null };

        var configs = await _sectionConfigRepository.GetAll();
        var config = configs.FirstOrDefault(c => c.Section == section);
        var deadline = timing.StartedAt.AddMinutes(config?.TimeLimitMinutes ?? 0);

        return new SectionStatusDto { IsCompleted = isCompleted, DeadlineUtc = deadline };
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
            OrderedAnswer = dto.OrderedAnswer,
            AudioAnswerUrl = dto.AudioAnswerUrl
        };

        await _candidateAnswerRepository.UpsertAsync(answer);
    }

    private async Task<(int Grammar, int Listening, int Reading, int Writing, int Speaking, int Total, LanguageLevel Level)>
        ComputeScoreAsync(Attempt attempt)
    {
        var answers = await _candidateAnswerRepository.GetByAttemptIdAsync(attempt.Id);
        var answerMap = answers.ToDictionary(a => a.QuestionId);
        var grades = await _manualGradeRepository.GetByAttemptIdAsync(attempt.Id);
        var gradeMap = grades.ToDictionary(g => g.QuestionId, g => g.Score);

        int grammarScore = 0, listeningScore = 0, readingScore = 0, writingScore = 0, speakingScore = 0;
        int maxPossiblePoints = 0;

        foreach (var tvq in attempt.TestVariant.Questions)
        {
            var question = tvq.Question;
            maxPossiblePoints += question.Type == QuestionType.OpenAnswer ? 5 : 1;

            int points;
            if (question.Type == QuestionType.OpenAnswer)
            {
                points = gradeMap.GetValueOrDefault(question.Id);
            }
            else
            {
                if (!answerMap.TryGetValue(question.Id, out var candidateAnswer))
                    continue;

                bool isCorrect = question.Type == QuestionType.MCQ
                    ? ScoringHelper.IsCorrectMcq(question, candidateAnswer)
                    : ScoringHelper.IsCorrectOrdered(question, candidateAnswer);
                points = isCorrect ? 1 : 0;
            }

            if (points == 0) continue;

            switch (question.Section)
            {
                case SectionType.Grammar: grammarScore += points; break;
                case SectionType.Listening: listeningScore += points; break;
                case SectionType.Reading: readingScore += points; break;
                case SectionType.Writing: writingScore += points; break;
                case SectionType.Speaking: speakingScore += points; break;
            }
        }

        int totalScore = grammarScore + listeningScore + readingScore + writingScore + speakingScore;
        double percentage = maxPossiblePoints > 0 ? (double)totalScore / maxPossiblePoints : 0;

        var level = percentage switch
        {
            < 0.10 => LanguageLevel.A1,
            < 0.35 => LanguageLevel.A2,
            < 0.60 => LanguageLevel.B1,
            _ => LanguageLevel.B2
        };

        return (grammarScore, listeningScore, readingScore, writingScore, speakingScore, totalScore, level);
    }

    public async Task RecomputeResultAsync(Guid attemptId)
    {
        var attempt = await _attemptRepository.GetByIdWithDetailsAsync(attemptId)
            ?? throw new NotFoundException($"Попытка {attemptId} не найдена");
        var result = await _resultRepository.GetByAttemptIdAsync(attemptId)
            ?? throw new NotFoundException($"Результат для попытки {attemptId} ещё не создан");

        var scores = await ComputeScoreAsync(attempt);
        result.Level = scores.Level;
        result.GrammarScore = scores.Grammar;
        result.ListeningScore = scores.Listening;
        result.ReadingScore = scores.Reading;
        result.WritingScore = scores.Writing;
        result.SpeakingScore = scores.Speaking;
        result.TotalScore = scores.Total;

        await _resultRepository.UpdateAsync(result);
    }

    public async Task<ResultResponseDto> SubmitAsync(Guid attemptId)
    {
        var attempt = await _attemptRepository.GetByIdWithDetailsAsync(attemptId)
            ?? throw new NotFoundException($"Попытка {attemptId} не найдена");

        var existingResult = await _resultRepository.GetByAttemptIdAsync(attemptId);
        if (existingResult != null)
            return await FinishCleanupAndMapAsync(attempt, existingResult);

        if (attempt.Status != AttemptStatus.InProgress)
            throw new BusinessException("Попытка не активна");

        var scores = await ComputeScoreAsync(attempt);

        var result = new Result
        {
            Id = Guid.NewGuid(),
            AttemptId = attemptId,
            CandidateId = attempt.CandidateId,
            Level = scores.Level,
            GrammarScore = scores.Grammar,
            ListeningScore = scores.Listening,
            ReadingScore = scores.Reading,
            WritingScore = scores.Writing,
            SpeakingScore = scores.Speaking,
            TotalScore = scores.Total,
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
        SpeakingScore = r.SpeakingScore,
        TotalScore = r.TotalScore,
        CreatedAt = r.CreatedAt
    };

    public async Task<bool> HasActiveAttemptAsync(Guid candidateId)
    {
        var active = await _attemptRepository.GetActiveByCandidate(candidateId);
        return active != null;
    }

    public async Task<List<ActiveAttemptDto>> GetActiveAttemptsAsync()
    {
        var attempts = await _attemptRepository.GetAllActiveWithDetailsAsync();
        var result = new List<ActiveAttemptDto>();

        foreach (var attempt in attempts)
        {
            var completedSections = await _completedSectionRepository.GetByAttemptIdAsync(attempt.Id);
            var completedSet = completedSections.Select(cs => cs.Section).ToHashSet();

            var currentSection = attempt.TestVariant.Questions
                .Select(tvq => tvq.Question.Section)
                .Distinct()
                .FirstOrDefault(s => !completedSet.Contains(s));

            var hasCurrentSection = attempt.TestVariant.Questions
                .Any(tvq => tvq.Question.Section == currentSection && !completedSet.Contains(currentSection));

            DateTime? deadline = null;
            if (hasCurrentSection)
            {
                var timing = await _sectionTimingRepository.GetByAttemptAndSectionAsync(attempt.Id, currentSection);
                if (timing != null)
                {
                    var configs = await _sectionConfigRepository.GetAll();
                    var config = configs.FirstOrDefault(c => c.Section == currentSection);
                    deadline = timing.StartedAt.AddMinutes(config?.TimeLimitMinutes ?? 0);
                }
            }

            result.Add(new ActiveAttemptDto
            {
                CandidateId = attempt.CandidateId,
                FullName = attempt.Candidate.FullName,
                Photo = attempt.Candidate.Photo,
                AttemptId = attempt.Id,
                AttemptStartedAt = attempt.StartedAt,
                CurrentSection = hasCurrentSection ? currentSection : null,
                SectionDeadlineUtc = deadline
            });
        }

        return result;
    }
}
