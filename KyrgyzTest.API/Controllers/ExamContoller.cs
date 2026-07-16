using KyrgyzTest.Application.DTOs;
using KyrgyzTest.Application.Interfaces;
using KyrgyzTest.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace KyrgyzTest.API.Controllers;

[ApiController]
[Route("api/exam")]
public class ExamController : ControllerBase
{
    private readonly ICandidateService _candidateService;
    private readonly IConfiguration _configuration;
    private readonly IExamService _examService;

    public ExamController(ICandidateService candidateService, IConfiguration configuration, IExamService examService)
    {
        _candidateService = candidateService;
        _configuration = configuration;
        _examService = examService;
    }

    [HttpPost("unlock")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Unlock([FromBody] UnlockDto dto)
    {
        var password = _configuration["ExamSettings:AccessPassword"];
        if (dto.Password != password)
            return Unauthorized("Неверный пароль");

        return Ok("Доступ открыт");
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(CandidateResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Login([FromBody] ExamLoginDto dto)
    {
        var candidate = await _candidateService.GetByAccessCodeAsync(dto.AccessCode);

        if (candidate == null)
            return NotFound("Кандидат не найден");

        if (candidate.BlockedUntil.HasValue && candidate.BlockedUntil.Value > DateTime.UtcNow)
            return StatusCode(403, $"Доступ заблокирован до {candidate.BlockedUntil.Value:dd.MM.yyyy}");

        if (!candidate.IsAllowed)
            return Forbid();

        return Ok(candidate);
    }

    [HttpPost("start")]
    [ProducesResponseType(typeof(StartExamResultDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Start([FromBody] StartExamRequestDto dto)
    {
        var result = await _examService.StartAsync(dto.CandidateId);
        return Ok(result);
    }

    [HttpPost("section/start")]
    [ProducesResponseType(typeof(ExamSectionDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> StartSection([FromBody] SectionRequestDto dto)
    {
        var result = await _examService.StartSectionAsync(dto.AttemptId, dto.Section);
        return Ok(result);
    }

    [HttpGet("section/status")]
    [ProducesResponseType(typeof(SectionStatusDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSectionStatus([FromQuery] Guid attemptId, [FromQuery] SectionType section)
    {
        var result = await _examService.GetSectionStatusAsync(attemptId, section);
        return Ok(result);
    }

    [HttpPost("section/submit")]
    [ProducesResponseType(typeof(SubmitSectionResultDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> SubmitSection([FromBody] SectionRequestDto dto)
    {
        var result = await _examService.SubmitSectionAsync(dto.AttemptId, dto.Section);
        return Ok(result);
    }

    [HttpPost("answer")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> SaveAnswer([FromBody] SaveAnswerDto dto)
    {
        await _examService.SaveAnswerAsync(dto);
        return Ok();
    }

    [HttpPost("submit")]
    [ProducesResponseType(typeof(ResultResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Submit([FromBody] SubmitExamDto dto)
    {
        var result = await _examService.SubmitAsync(dto.AttemptId);
        return Ok(result);
    }
}
