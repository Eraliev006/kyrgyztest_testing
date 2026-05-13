using KyrgyzTest.Application.DTOs;

namespace KyrgyzTest.Application.Interfaces;

public interface ISectionConfigService
{
    Task<SectionConfigResponseDto?> GetById(Guid id);
    Task<List<SectionConfigResponseDto>> GetAll();
    Task<SectionConfigResponseDto> Create(CreateSectionConfigDto dto);
    Task<SectionConfigResponseDto> Update(Guid id, CreateSectionConfigDto dto);
    Task<SectionConfigResponseDto> Delete(Guid id);
}
