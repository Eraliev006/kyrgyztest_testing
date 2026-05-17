using KyrgyzTest.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KyrgyzTest.API.Controllers;

[ApiController]
[Route("api/audit")]
[Authorize(Roles = "SuperAdmin,Director")]
public class AuditController : ControllerBase
{
    private readonly IAuditService _auditService;

    public AuditController(IAuditService auditService)
    {
        _auditService = auditService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAuditLogs(
        [FromQuery] DateTime? dateFrom,
        [FromQuery] DateTime? dateTo,
        [FromQuery] Guid? userId,
        [FromQuery] string? entityType)
    {
        var logs = await _auditService.GetFilteredAsync(dateFrom, dateTo, userId, entityType);
        return Ok(logs);
    }
}
