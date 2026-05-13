using KyrgyzTest.Application.DTOs;
using KyrgyzTest.Application.Interfaces;
using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Interfaces;

namespace KyrgyzTest.Application.Services;

public class SectionConfigService : ISectionConfigService
{
    private readonly ISectionConfigRepository _repository;

    public SectionConfigService(ISectionConfigRepository repository)
    {
        _repository = repository;
    }

    public async Task<SectionConfigResponseDto?> GetById(Guid id)
    {
        var config = await _repository.GetById(id);
        return config == null ? null : Map(config);
    }

    public async Task<List<SectionConfigResponseDto>> GetAll()
    {
        var configs = await _repository.GetAll();
        return configs.Select(Map).ToList();
    }

    public async Task<SectionConfigResponseDto> Create(CreateSectionConfigDto dto)
    {
        var config = new SectionConfig
        {
            Id = Guid.NewGuid(),
            Section = dto.Section,
            TimeLimitMinutes = dto.TimeLimitMinutes,
            A1Count = dto.A1Count,
            A2Count = dto.A2Count,
            B1Count = dto.B1Count,
            B2Count = dto.B2Count
        };

        var created = await _repository.Create(config);
        return Map(created);
    }

    public async Task<SectionConfigResponseDto> Update(Guid id, CreateSectionConfigDto dto)
    {
        var config = await _repository.GetById(id);

        if (config == null)
            throw new Exception("SectionConfig not found");

        config.Section = dto.Section;
        config.TimeLimitMinutes = dto.TimeLimitMinutes;
        config.A1Count = dto.A1Count;
        config.A2Count = dto.A2Count;
        config.B1Count = dto.B1Count;
        config.B2Count = dto.B2Count;

        var updated = await _repository.Update(config);
        return Map(updated);
    }

    public async Task<SectionConfigResponseDto> Delete(Guid id)
    {
        var deleted = await _repository.Delete(id);
        return Map(deleted);
    }

    private static SectionConfigResponseDto Map(SectionConfig config)
    {
        return new SectionConfigResponseDto
        {
            Id = config.Id,
            Section = config.Section,
            TimeLimitMinutes = config.TimeLimitMinutes,
            A1Count = config.A1Count,
            A2Count = config.A2Count,
            B1Count = config.B1Count,
            B2Count = config.B2Count
        };
    }
}
