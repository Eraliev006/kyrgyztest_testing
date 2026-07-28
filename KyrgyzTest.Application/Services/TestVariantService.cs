using KyrgyzTest.Application.DTOs;
using KyrgyzTest.Application.Interfaces;
using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Enums;
using KyrgyzTest.Core.Exceptions;
using KyrgyzTest.Core.Interfaces;

namespace KyrgyzTest.Application.Services;

public class TestVariantService : ITestVariantService
{
    private readonly ITestVariantRepository _variantRepo;
    private readonly IQuestionRepository _questionRepo;
    private readonly IAttemptRepository _attemptRepo;
    private readonly ITestVariantGeneratorService _generatorService;
    private readonly IAuditService _audit;

    public TestVariantService(
        ITestVariantRepository variantRepo,
        IQuestionRepository questionRepo,
        IAttemptRepository attemptRepo,
        ITestVariantGeneratorService generatorService,
        IAuditService audit)
    {
        _variantRepo = variantRepo;
        _questionRepo = questionRepo;
        _attemptRepo = attemptRepo;
        _generatorService = generatorService;
        _audit = audit;
    }

    public async Task<List<TestVariantSummaryDto>> GetAllAsync(bool includeArchived = false)
    {
        var variants = await _variantRepo.GetAllAsync(includeArchived);
        return variants.Select(v => new TestVariantSummaryDto
        {
            Id = v.Id,
            Number = v.Number,
            GeneratedAt = v.GeneratedAt,
            QuestionCount = v.Questions.Count,
            IsArchived = v.IsArchived
        }).ToList();
    }

    public async Task<TestVariantSummaryDto> GenerateAsync()
    {
        var variant = await _generatorService.GenerateAsync();
        await _audit.LogAsync("GENERATE", "TestVariant", variant.Id, $"Сгенерирован вариант №{variant.Number}");
        return new TestVariantSummaryDto
        {
            Id = variant.Id,
            Number = variant.Number,
            GeneratedAt = variant.GeneratedAt,
            QuestionCount = variant.Questions.Count,
            IsArchived = variant.IsArchived
        };
    }

    public async Task DeleteAsync(Guid id)
    {
        var variant = await _variantRepo.GetByIdAsync(id)
            ?? throw new NotFoundException("Вариант не найден");

        if (await _attemptRepo.ExistsByTestVariantIdAsync(id))
            throw new BusinessException("Нельзя удалить вариант — по нему уже есть попытки кандидатов. Можно только архивировать.");

        await _variantRepo.DeleteAsync(variant);
        await _audit.LogAsync("DELETE", "TestVariant", id, $"Удалён вариант №{variant.Number}");
    }

    public async Task<TestVariantDetailDto> GetByIdAsync(Guid id)
    {
        var variant = await _variantRepo.GetByIdAsync(id)
            ?? throw new NotFoundException("Вариант не найден");

        var questionsBySection = variant.Questions
            .GroupBy(vq => vq.Question.Section.ToString())
            .ToDictionary(
                g => g.Key,
                g => g.Select(vq => MapQuestion(vq)).ToList()
            );

        return new TestVariantDetailDto
        {
            Id = variant.Id,
            GeneratedAt = variant.GeneratedAt,
            QuestionsBySection = questionsBySection
        };
    }

    public async Task ReplaceQuestionAsync(Guid variantId, Guid questionId, Guid newQuestionId)
    {
        var variant = await _variantRepo.GetByIdAsync(variantId)
            ?? throw new NotFoundException("Вариант не найден");

        var tvq = variant.Questions.FirstOrDefault(vq => vq.QuestionId == questionId)
            ?? throw new NotFoundException("Вопрос не найден в варианте");

        var newQuestion = await _questionRepo.GetByIdAsync(newQuestionId)
            ?? throw new NotFoundException("Новый вопрос не найден");

        if (newQuestion.Section != tvq.Question.Section || newQuestion.Level != tvq.Question.Level || newQuestion.Type != tvq.Question.Type)
            throw new ValidationException("Новый вопрос должен быть той же секции, уровня и типа");

        tvq.QuestionId = newQuestionId;
        await _variantRepo.SaveAsync();
        await _audit.LogAsync("REPLACE_QUESTION", "TestVariant", variantId, $"В варианте заменён вопрос {questionId} на {newQuestionId}");
    }

    public async Task<List<VariantQuestionDto>> GetAvailableQuestionsAsync(Guid variantId, SectionType section, LanguageLevel level)
    {
        var variant = await _variantRepo.GetByIdAsync(variantId)
            ?? throw new NotFoundException("Вариант не найден");

        var existingIds = variant.Questions.Select(vq => vq.QuestionId).ToHashSet();
        var questions = await _questionRepo.GetBySectionAndLevelAsync(section, level);

        return questions
            .Where(q => !existingIds.Contains(q.Id))
            .Select(q => new VariantQuestionDto
            {
                Id = q.Id,
                Content = q.Content,
                Section = q.Section.ToString(),
                Level = q.Level.ToString(),
                Type = q.Type.ToString()
            })
            .ToList();
    }

    private static VariantQuestionDto MapQuestion(TestVariantQuestion tvq) => new()
    {
        Id = tvq.Question.Id,
        Content = tvq.Question.Content,
        Section = tvq.Question.Section.ToString(),
        Level = tvq.Question.Level.ToString(),
        Type = tvq.Question.Type.ToString()
    };
}
