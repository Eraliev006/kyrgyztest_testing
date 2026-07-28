using KyrgyzTest.Application.DTOs;
using KyrgyzTest.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KyrgyzTest.API.Controllers;

[ApiController]
[Route("api/password-reset-requests")]
public class PasswordResetController : ControllerBase
{
    private readonly IPasswordResetService _service;

    public PasswordResetController(IPasswordResetService service)
    {
        _service = service;
    }

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Request([FromBody] RequestPasswordResetDto dto)
    {
        await _service.RequestResetAsync(dto);
        return NoContent();
    }

    [HttpGet]
    [Authorize(Roles = "SuperAdmin,Director")]
    [ProducesResponseType(typeof(List<PasswordResetRequestResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPending()
    {
        var requests = await _service.GetPendingAsync();
        return Ok(requests);
    }

    [HttpPut("{id:guid}/approve")]
    [Authorize(Roles = "SuperAdmin,Director")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Approve(Guid id)
    {
        await _service.ApproveAsync(id);
        return NoContent();
    }

    [HttpPut("{id:guid}/reject")]
    [Authorize(Roles = "SuperAdmin,Director")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Reject(Guid id)
    {
        await _service.RejectAsync(id);
        return NoContent();
    }
}
