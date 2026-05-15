using KyrgyzTest.Core.Entities;

namespace KyrgyzTest.Core.Interfaces;

public interface IOrganizationRepository
{
    Task<Organization?> GetByIdAsync(Guid id);
    Task<List<Organization>> GetAllAsync();
    Task<Organization> CreateAsync(Organization organization);
    Task<Organization> UpdateAsync(Organization organization);
    Task DeleteAsync(Guid id);
}
