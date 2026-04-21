using KyrgyzTest.Application.DTOs;
using KyrgyzTest.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KyrgyzTest.API.Controllers;

[ApiController]
[Route("api/exam")]
public class ExamController : ControllerBase
{
    private readonly ICandidateService _candidateService;
    private readonly IConfiguration _configuration;

    public ExamController(ICandidateService candidateService, IConfiguration configuration)
    {
        _candidateService = candidateService;
        _configuration = configuration;
    }

    [HttpPost("unlock")]
    public IActionResult Unlock([FromBody] UnlockDto dto)
    {
        var password = _configuration["ExamSettings:AccessPassword"];
        if (dto.Password != password)
            return Unauthorized("Неверный пароль");

        return Ok("Доступ открыт");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] ExamLoginDto dto)
    {
        var candidate = await _candidateService.GetByAccessCodeAsync(dto.AccessCode);

        if (candidate == null)
            return NotFound("Кандидат не найден");

        if (!candidate.IsAllowed)
            return Forbid();

        return Ok(candidate);
    }
}