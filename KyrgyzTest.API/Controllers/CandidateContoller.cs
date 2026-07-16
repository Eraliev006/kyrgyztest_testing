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
    private readonly IExamService _examService;

    public CandidateController(ICandidateService service, IExamService examService)
    {
        _service = service;
        _examService = examService;
    }

    [HttpGet("active-attempts")]
    [Authorize(Roles = "SuperAdmin,Director,Admin")]
    [ProducesResponseType(typeof(List<ActiveAttemptDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActiveAttempts()
    {
        var result = await _examService.GetActiveAttemptsAsync();
        return Ok(result);
    }

    [HttpGet]
    [Authorize(Roles = "SuperAdmin,Director,Admin")]
    [ProducesResponseType(typeof(PagedResultDto<CandidateResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaged(
        [FromQuery] Guid? organizationId,
        [FromQuery] DateTime? dateFrom,
        [FromQuery] DateTime? dateTo,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var result = await _service.GetPagedAsync(organizationId, dateFrom, dateTo, page, pageSize);
        return Ok(result);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string? inn, [FromQuery] string? code, [FromQuery] string? fullName)
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

        if (!string.IsNullOrEmpty(fullName))
        {
            var byName = await _service.SearchByNameAsync(fullName);
            return Ok(byName);
        }

        return Ok(Array.Empty<object>());
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CandidateResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var candidate = await _service.GetByIdAsync(id);
        return candidate == null ? NotFound() : Ok(candidate);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CandidateResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Create([FromBody] CreateCandidateDto dto)
    {
        var candidate = await _service.CreateAsync(dto);
        return Ok(candidate);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "SuperAdmin,Director,Admin")]
    [ProducesResponseType(typeof(CandidateResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCandidateDto dto)
    {
        var candidate = await _service.UpdateAsync(id, dto);
        return Ok(candidate);
    }

    [HttpPut("{id}/photo")]
    [Authorize(Roles = "SuperAdmin,Director,Admin")]
    [ProducesResponseType(typeof(CandidateResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> UploadPhoto(Guid id, [FromBody] UploadPhotoDto dto)
    {
        var candidate = await _service.UploadPhotoAsync(id, dto.Photo);
        return Ok(candidate);
    }

    [HttpPut("{id}/allow")]
    [Authorize(Roles = "SuperAdmin,Director,Admin")]
    [ProducesResponseType(typeof(CandidateResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> AllowAccess(Guid id)
    {
        var candidate = await _service.AllowAccessAsync(id);
        return Ok(candidate);
    }

    [HttpPut("{id}/deny")]
    [Authorize(Roles = "SuperAdmin,Director,Admin")]
    [ProducesResponseType(typeof(CandidateResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> DenyAccess(Guid id)
    {
        var candidate = await _service.DenyAccessAsync(id);
        return Ok(candidate);
    }

    [HttpPut("{id}/block")]
    [Authorize(Roles = "SuperAdmin,Director,Admin")]
    [ProducesResponseType(typeof(CandidateResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Block(Guid id, [FromBody] BlockCandidateDto dto)
    {
        var candidate = await _service.BlockAsync(id, dto);
        return Ok(candidate);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "SuperAdmin,Director,Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}