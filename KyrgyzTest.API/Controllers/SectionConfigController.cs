using KyrgyzTest.Application.DTOs;
using KyrgyzTest.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KyrgyzTest.API.Controllers;

[ApiController]
[Route("api/section-config")]
[Authorize(Roles = "SuperAdmin,Director")]
public class SectionConfigController : ControllerBase
{
    private readonly ISectionConfigService _service;

    public SectionConfigController(ISectionConfigService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<SectionConfigResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAll());
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(SectionConfigResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var config = await _service.GetById(id);
        return config == null ? NotFound() : Ok(config);
    }

    [HttpPost]
    [ProducesResponseType(typeof(SectionConfigResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Create(CreateSectionConfigDto dto)
    {
        var config = await _service.Create(dto);
        return Ok(config);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(SectionConfigResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(Guid id, CreateSectionConfigDto dto)
    {
        var config = await _service.Update(id, dto);
        return Ok(config);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(SectionConfigResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var config = await _service.Delete(id);
        return Ok(config);
    }
}
