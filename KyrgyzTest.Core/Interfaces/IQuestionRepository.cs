using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Enums;

namespace KyrgyzTest.Core.Interfaces;

public interface IQuestionRepository
{
    Task<Question?> GetByIdAsync(Guid id);
    Task<List<Question>> GetAllAsync();
    Task<List<Question>> GetBySectionAsync(SectionType section);
    Task<List<Question>> GetBySectionAndLevelAsync(SectionType section, LanguageLevel level);
    Task<Question> CreateAsync(Question question);
    Task<Question> UpdateAsync(Question question);
    Task DeleteAsync(Guid id);
    Task<Dictionary<Guid, int>> GetVariantUsageCountsAsync(IEnumerable<Guid> questionIds);
}