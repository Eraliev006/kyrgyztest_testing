using KyrgyzTest.Application.DTOs;
using KyrgyzTest.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KyrgyzTest.API.Controllers;

[ApiController]
[Route("api/candidates")]
[Authorize]
public class CandidateController : ControllerBase
{
    private readonly ICandidateService _service;

    public CandidateController(ICandidateService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var candidate = await _service.GetByIdAsync(id);
        return candidate == null ? NotFound() : Ok(candidate);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string? inn, [FromQuery] string? code)
    {
        if (!string.IsNullOrEmpty(inn))
        {
            var byInn = await _service.GetByInnAsync(inn);
            return byInn == null ? NotFound() : Ok(byInn);
        }

        if (!string.IsNullOrEmpty(code))
        {
            var byCode = await _service.GetByAccessCodeAsync(code);
            return byCode == null ? NotFound() : Ok(byCode);
        }

        return BadRequest("Укажите inn или code");
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCandidateDto dto)
    {
        var candidate = await _service.CreateAsync(dto);
        return Ok(candidate);
    }

    [HttpPut("{id}/allow")]
    public async Task<IActionResult> AllowAccess(Guid id)
    {
        var candidate = await _service.AllowAccessAsync(id);
        return Ok(candidate);
    }
}