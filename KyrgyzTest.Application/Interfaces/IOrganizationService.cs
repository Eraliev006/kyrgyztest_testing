using KyrgyzTest.Application.DTOs;

namespace KyrgyzTest.Application.Interfaces;

public interface IOrganizationService
{
    Task<List<OrganizationDto>> GetAllAsync();
    Task<OrganizationDto> CreateAsync(CreateOrganizationDto dto);
    Task<OrganizationDto> UpdateAsync(Guid id, CreateOrganizationDto dto);
    Task DeleteAsync(Guid id);
}
