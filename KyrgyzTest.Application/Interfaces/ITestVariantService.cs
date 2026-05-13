using KyrgyzTest.Application.DTOs;
using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Enums;

namespace KyrgyzTest.Application.Interfaces;

public interface ITestVariantService
{
    Task<List<TestVariantSummaryDto>> GetAllAsync();
    Task<TestVariantDetailDto> GetByIdAsync(Guid id);
    Task ReplaceQuestionAsync(Guid variantId, Guid questionId, Guid newQuestionId);
    Task<List<VariantQuestionDto>> GetAvailableQuestionsAsync(Guid variantId, SectionType section, LanguageLevel level);
}
