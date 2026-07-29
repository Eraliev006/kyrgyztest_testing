using KyrgyzTest.Application.DTOs;
using KyrgyzTest.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KyrgyzTest.API.Controllers;

[ApiController]
[Route("api/exam-settings/access-password")]
[Authorize(Roles = "SuperAdmin,Director")]
public class ExamAccessSettingsController : ControllerBase
{
    private readonly IExamAccessSettingsService _service;

    public ExamAccessSettingsController(IExamAccessSettingsService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ExamAccessSettingsDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        return Ok(await _service.GetAsync());
    }

    [HttpPut]
    [ProducesResponseType(typeof(ExamAccessSettingsDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(UpdateExamAccessPasswordDto dto)
    {
        return Ok(await _service.UpdateAsync(dto));
    }
}
