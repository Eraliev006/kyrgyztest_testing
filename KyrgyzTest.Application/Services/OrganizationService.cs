using KyrgyzTest.Application.DTOs;
using KyrgyzTest.Application.Interfaces;
using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Exceptions;
using KyrgyzTest.Core.Interfaces;

namespace KyrgyzTest.Application.Services;

public class OrganizationService : IOrganizationService
{
    private readonly IOrganizationRepository _repository;

    public OrganizationService(IOrganizationRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<OrganizationDto>> GetAllAsync()
    {
        var orgs = await _repository.GetAllAsync();
        return orgs.Select(Map).ToList();
    }

    public async Task<OrganizationDto> CreateAsync(CreateOrganizationDto dto)
    {
        var org = new Organization
        {
            Id = Guid.NewGuid(),
            NameRu = dto.NameRu,
            NameKg = dto.NameKg,
            ShortName = dto.ShortName
        };
        var created = await _repository.CreateAsync(org);
        return Map(created);
    }

    public async Task<OrganizationDto> UpdateAsync(Guid id, CreateOrganizationDto dto)
    {
        var org = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException("Организация не найдена");

        org.NameRu = dto.NameRu;
        org.NameKg = dto.NameKg;
        org.ShortName = dto.ShortName;

        var updated = await _repository.UpdateAsync(org);
        return Map(updated);
    }

    public async Task DeleteAsync(Guid id)
    {
        _ = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException("Организация не найдена");
        await _repository.DeleteAsync(id);
    }

    private static OrganizationDto Map(Organization o) => new()
    {
        Id = o.Id,
        NameRu = o.NameRu,
        NameKg = o.NameKg,
        ShortName = o.ShortName
    };
}
