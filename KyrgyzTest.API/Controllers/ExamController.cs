using KyrgyzTest.Application.DTOs;
using KyrgyzTest.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KyrgyzTest.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExamController: ControllerBase
{
    private readonly IExamService _examService;
    
    public ExamController(IExamService examService)
    {
        _examService = examService;
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<ExamLoginResponseDto>> Login(ExamLoginDto examLogin)
    {
        var result = await _examService.LoginAsync(examLogin);
        return Ok(result);
    }

    [HttpGet("session/{examCode}")]
    public async Task<ActionResult<ExamLoginResponseDto>> GetSession(string examCode)
    {
        var result = await _examService.GetSessionAsync(examCode);
        return Ok(result);
    }
}