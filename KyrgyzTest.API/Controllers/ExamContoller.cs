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
    private readonly IExamService _examService;

    public ExamController(ICandidateService candidateService, IConfiguration configuration, IExamService examService)
    {
        _candidateService = candidateService;
        _configuration = configuration;
        _examService = examService;
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

    [HttpPost("start")]
    public async Task<IActionResult> Start([FromBody] StartExamRequestDto dto)
    {
        var result = await _examService.StartAsync(dto.CandidateId);
        return Ok(result);
    }

    [HttpPost("section/start")]
    public async Task<IActionResult> StartSection([FromBody] SectionRequestDto dto)
    {
        var result = await _examService.StartSectionAsync(dto.AttemptId, dto.Section);
        return Ok(result);
    }

    [HttpPost("section/submit")]
    public async Task<IActionResult> SubmitSection([FromBody] SectionRequestDto dto)
    {
        var result = await _examService.SubmitSectionAsync(dto.AttemptId, dto.Section);
        return Ok(result);
    }

    [HttpPost("answer")]
    public async Task<IActionResult> SaveAnswer([FromBody] SaveAnswerDto dto)
    {
        await _examService.SaveAnswerAsync(dto);
        return Ok();
    }

    [HttpPost("submit")]
    public async Task<IActionResult> Submit([FromBody] SubmitExamDto dto)
    {
        var result = await _examService.SubmitAsync(dto.AttemptId);
        return Ok(result);
    }
}