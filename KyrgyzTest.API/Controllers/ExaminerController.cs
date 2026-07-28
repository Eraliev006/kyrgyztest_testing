using System.Security.Claims;
using KyrgyzTest.Application.DTOs;
using KyrgyzTest.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KyrgyzTest.API.Controllers;

[ApiController]
[Route("api/examiner")]
[Authorize(Roles = "SuperAdmin,Examiner")]
public class ExaminerController : ControllerBase
{
    private readonly IExaminerService _examinerService;

    public ExaminerController(IExaminerService examinerService)
    {
        _examinerService = examinerService;
    }

    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    [HttpGet("queue")]
    [ProducesResponseType(typeof(List<ExaminerQueueItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetQueue()
    {
        var result = await _examinerService.GetQueueAsync();
        return Ok(result);
    }

    [HttpGet("review/{attemptId}")]
    [ProducesResponseType(typeof(ExaminerReviewDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetReview(Guid attemptId)
    {
        var result = await _examinerService.GetReviewAsync(attemptId);
        return Ok(result);
    }

    [HttpPost("grade")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Grade([FromBody] GradeAnswerDto dto)
    {
        await _examinerService.GradeAsync(dto, CurrentUserId);
        return Ok();
    }
}
