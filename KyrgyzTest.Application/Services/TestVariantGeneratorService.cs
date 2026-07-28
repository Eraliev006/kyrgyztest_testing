using KyrgyzTest.Application.Interfaces;
using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Enums;
using KyrgyzTest.Core.Interfaces;

namespace KyrgyzTest.Application.Services;

public class TestVariantGeneratorService : ITestVariantGeneratorService
{
    private readonly ISectionConfigRepository _sectionConfigRepository;
    private readonly IQuestionRepository _questionRepository;
    private readonly ITestVariantRepository _testVariantRepository;
    private static readonly Random _random = new();

    public TestVariantGeneratorService(
        ISectionConfigRepository sectionConfigRepository,
        IQuestionRepository questionRepository,
        ITestVariantRepository testVariantRepository)
    {
        _sectionConfigRepository = sectionConfigRepository;
        _questionRepository = questionRepository;
        _testVariantRepository = testVariantRepository;
    }

    public async Task<TestVariant> GenerateAsync()
    {
        var configs = await _sectionConfigRepository.GetAll();
        var tvQuestions = new List<TestVariantQuestion>();
        int orderIndex = 0;

        foreach (var config in configs)
        {
            var levelCounts = new[]
            {
                (LanguageLevel.A1, config.A1Count),
                (LanguageLevel.A2, config.A2Count),
                (LanguageLevel.B1, config.B1Count),
                (LanguageLevel.B2, config.B2Count),
            };

            foreach (var (level, count) in levelCounts)
            {
                if (count <= 0) continue;

                var questions = await _questionRepository.GetBySectionAndLevelAsync(config.Section, level);
                var picked = questions.OrderBy(_ => _random.Next()).Take(count);

                foreach (var question in picked)
                {
                    tvQuestions.Add(new TestVariantQuestion
                    {
                        Id = Guid.NewGuid(),
                        QuestionId = question.Id,
                        Question = question,
                        OrderIndex = orderIndex++
                    });
                }
            }
        }

        var maxNumber = await _testVariantRepository.GetMaxNumberAsync();

        var variant = new TestVariant
        {
            Id = Guid.NewGuid(),
            Number = maxNumber + 1,
            GeneratedAt = DateTime.UtcNow,
            IsArchived = false,
            Questions = tvQuestions
        };

        await _testVariantRepository.AddAsync(variant);
        return variant;
    }
}
