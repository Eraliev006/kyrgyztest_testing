using KyrgyzTest.Core.Entities;

namespace KyrgyzTest.Core.Interfaces;

public interface ITestVariantRepository
{
    Task<List<TestVariant>> GetAllAsync(bool includeArchived = false);
    Task<TestVariant?> GetByIdAsync(Guid id);
    Task<TestVariant?> GetRandomActiveAsync();
    Task<int> GetMaxNumberAsync();
    Task AddAsync(TestVariant variant);
    Task DeleteAsync(TestVariant variant);
    Task SaveAsync();
}
