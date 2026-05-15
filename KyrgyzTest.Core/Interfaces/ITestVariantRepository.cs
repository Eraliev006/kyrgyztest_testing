using KyrgyzTest.Core.Entities;

namespace KyrgyzTest.Core.Interfaces;

public interface ITestVariantRepository
{
    Task<List<TestVariant>> GetAllAsync();
    Task<TestVariant?> GetByIdAsync(Guid id);
    Task<int> GetMaxNumberAsync();
    Task AddAsync(TestVariant variant);
    Task SaveAsync();
}
