using KyrgyzTest.Application.Interfaces;
using KyrgyzTest.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KyrgyzTest.API.Controllers;

[ApiController]
[Route("api/results")]
[Authorize]
public class ResultsController : ControllerBase
{
    private readonly IResultService _service;

    public ResultsController(IResultService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] Guid? organizationId,
        [FromQuery] LanguageLevel? level,
        [FromQuery] DateTime? dateFrom,
        [FromQuery] DateTime? dateTo)
    {
        var results = await _service.GetAllAsync(organizationId, level, dateFrom, dateTo);
        return Ok(results);
    }

    [HttpGet("{candidateId:guid}")]
    public async Task<IActionResult> GetByCandidate(Guid candidateId)
    {
        var result = await _service.GetByCandidateIdAsync(candidateId);
        return Ok(result);
    }

    [HttpGet("{attemptId:guid}/details")]
    [Authorize(Roles = "SuperAdmin,Director,Admin")]
    public async Task<IActionResult> GetAttemptDetails(Guid attemptId)
    {
        var details = await _service.GetAttemptDetailsAsync(attemptId);
        return Ok(details);
    }

    [HttpGet("stats")]
    public async Task<IActionResult> GetStats(
        [FromQuery] Guid? organizationId,
        [FromQuery] DateTime? dateFrom,
        [FromQuery] DateTime? dateTo)
    {
        var stats = await _service.GetStatsAsync(organizationId, dateFrom, dateTo);
        return Ok(stats);
    }
}
