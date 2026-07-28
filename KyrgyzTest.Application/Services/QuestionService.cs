using KyrgyzTest.Application.DTOs;
using KyrgyzTest.Application.Interfaces;
using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Enums;
using KyrgyzTest.Core.Exceptions;
using KyrgyzTest.Core.Interfaces;

namespace KyrgyzTest.Application.Services;

public class QuestionService : IQuestionService
{
    private readonly IQuestionRepository _questionRepository;
    private readonly IMediaGroupRepository _mediaGroupRepository;
    private readonly IAuditService _audit;

    public QuestionService(
        IQuestionRepository questionRepository,
        IMediaGroupRepository mediaGroupRepository,
        IAuditService audit)
    {
        _questionRepository = questionRepository;
        _mediaGroupRepository = mediaGroupRepository;
        _audit = audit;
    }

    public async Task<List<QuestionResponseDto>> GetAllAsync(SectionType? section, LanguageLevel? level)
    {
        List<Question> questions;

        if (section.HasValue && level.HasValue)
            questions = await _questionRepository.GetBySectionAndLevelAsync(section.Value, level.Value);
        else if (section.HasValue)
            questions = await _questionRepository.GetBySectionAsync(section.Value);
        else
            questions = await _questionRepository.GetAllAsync();

        if (level.HasValue && !section.HasValue)
            questions = questions.Where(q => q.Level == level.Value).ToList();

        var usageCounts = await _questionRepository.GetVariantUsageCountsAsync(questions.Select(q => q.Id));
        return questions.Select(q => Map(q, usageCounts.GetValueOrDefault(q.Id))).ToList();
    }

    public async Task<QuestionResponseDto?> GetByIdAsync(Guid id)
    {
        var question = await _questionRepository.GetByIdAsync(id);
        if (question == null) return null;

        var usageCounts = await _questionRepository.GetVariantUsageCountsAsync([id]);
        return Map(question, usageCounts.GetValueOrDefault(id));
    }

    public async Task<QuestionResponseDto> CreateAsync(CreateQuestionDto dto)
    {
        var question = new Question
        {
            Id = Guid.NewGuid(),
            Section = dto.Section,
            Level = dto.Level,
            Type = dto.Type,
            Content = dto.Content,
            MediaGroupId = dto.MediaGroupId,
            TopicId = dto.TopicId,
            OrderIndex = 0,
            CreatedAt = DateTime.UtcNow,
            AnswerOptions = dto.AnswerOptions.Select(a => new AnswerOption
            {
                Id = Guid.NewGuid(),
                Content = a.Content,
                IsCorrect = a.IsCorrect,
                OrderIndex = a.OrderIndex
            }).ToList()
        };

        var created = await _questionRepository.CreateAsync(question);
        await _audit.LogAsync("CREATE", "Question", created.Id, $"Создан вопрос (секция: {created.Section}, уровень: {created.Level})");
        return Map(created);
    }

    public async Task<QuestionResponseDto> UpdateAsync(Guid id, CreateQuestionDto dto)
    {
        var question = await _questionRepository.GetByIdAsync(id)
                       ?? throw new NotFoundException("Вопрос не найден");

        question.Section = dto.Section;
        question.Level = dto.Level;
        question.Type = dto.Type;
        question.Content = dto.Content;
        question.MediaGroupId = dto.MediaGroupId;
        question.TopicId = dto.TopicId;
        question.AnswerOptions = dto.AnswerOptions.Select(a => new AnswerOption
        {
            Id = Guid.NewGuid(),
            QuestionId = question.Id,
            Content = a.Content,
            IsCorrect = a.IsCorrect,
            OrderIndex = a.OrderIndex
        }).ToList();

        var updated = await _questionRepository.UpdateAsync(question);
        await _audit.LogAsync("UPDATE", "Question", id, $"Обновлён вопрос (секция: {updated.Section}, уровень: {updated.Level})");

        var usageCounts = await _questionRepository.GetVariantUsageCountsAsync([id]);
        return Map(updated, usageCounts.GetValueOrDefault(id));
    }

    public async Task DeleteAsync(Guid id)
    {
        var question = await _questionRepository.GetByIdAsync(id)
                       ?? throw new NotFoundException("Вопрос не найден");

        await _questionRepository.DeleteAsync(question.Id);
        await _audit.LogAsync("DELETE", "Question", id, $"Удалён вопрос (секция: {question.Section}, уровень: {question.Level})");
    }

    public async Task<MediaGroupResponseDto> CreateMediaGroupAsync(CreateMediaGroupDto dto)
    {
        var mediaGroup = new MediaGroup
        {
            Id = Guid.NewGuid(),
            Type = dto.Type,
            Content = dto.Content
        };

        var created = await _mediaGroupRepository.CreateAsync(mediaGroup);

        return new MediaGroupResponseDto
        {
            Id = created.Id,
            Type = created.Type,
            Content = created.Content
        };
    }

    private static QuestionResponseDto Map(Question q, int usedInVariantCount = 0) => new()
    {
        Id = q.Id,
        Section = q.Section,
        Level = q.Level,
        Type = q.Type,
        Content = q.Content,
        MediaGroupId = q.MediaGroupId,
        MediaUrl = q.MediaGroup?.Type == MediaType.Audio ? q.MediaGroup.Content : null,
        MediaText = q.MediaGroup?.Type == MediaType.Text ? q.MediaGroup.Content : null,
        TopicId = q.TopicId,
        TopicName = q.Topic?.Name,
        CreatedAt = q.CreatedAt,
        UsedInVariantCount = usedInVariantCount,
        AnswerOptions = q.AnswerOptions.Select(a => new AnswerOptionResponseDto
        {
            Id = a.Id,
            Content = a.Content,
            IsCorrect = a.IsCorrect,
            OrderIndex = a.OrderIndex
        }).ToList()
    };
}