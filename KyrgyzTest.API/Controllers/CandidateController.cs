using KyrgyzTest.Application.DTOs;
using KyrgyzTest.Application.Interfaces;
using KyrgyzTest.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace KyrgyzTest.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CandidateController: ControllerBase
{
    private readonly IExamService _examService;
    
    public CandidateController(IExamService examService)
    {
        _examService = examService;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> RegisterCandidate(RegisterCandidateDto candidateDto)
    {
        var result = await _examService.RegisterCandidateAsync(candidateDto);
        return Ok(result);
    }
    
    [HttpPost("assign")]
    public async Task<IActionResult> AssignComputer([FromQuery] string examCode)
    {
        var result = await _examService.AssignComputerAsync(examCode);
        return Ok(result);
    }
}