using KyrgyzTest.Application.DTOs;
using KyrgyzTest.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KyrgyzTest.API.Controllers;

[ApiController]
[Route("api/organizations")]
[Authorize]
public class OrganizationController : ControllerBase
{
    private readonly IOrganizationService _service;

    public OrganizationController(IOrganizationService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _service.GetAllAsync());

    [HttpPost]
    [Authorize(Roles = "SuperAdmin,Director")]
    public async Task<IActionResult> Create([FromBody] CreateOrganizationDto dto)
    {
        var org = await _service.CreateAsync(dto);
        return Ok(org);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "SuperAdmin,Director")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CreateOrganizationDto dto)
    {
        var org = await _service.UpdateAsync(id, dto);
        return Ok(org);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "SuperAdmin,Director")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
