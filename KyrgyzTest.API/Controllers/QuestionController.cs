using KyrgyzTest.Application.DTOs;
using KyrgyzTest.Application.Interfaces;
using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KyrgyzTest.API.Controllers;


[ApiController]
[Route("api/questions")]
[Authorize(Roles = "SuperAdmin,Expert")]
public class QuestionController : ControllerBase
{
    private readonly IQuestionService _service;

    public QuestionController(IQuestionService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<QuestionResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] SectionType? section, [FromQuery] LanguageLevel? level)
        => Ok(await _service.GetAllAsync(section, level));

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(QuestionResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var question = await _service.GetByIdAsync(id);
        return question == null ? NotFound() : Ok(question);
    }

    [HttpPost]
    [ProducesResponseType(typeof(QuestionResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Create([FromBody] CreateQuestionDto dto)
        => Ok(await _service.CreateAsync(dto));

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(QuestionResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(Guid id, [FromBody] CreateQuestionDto dto)
        => Ok(await _service.UpdateAsync(id, dto));

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }

    [HttpPost("media-group")]
    [ProducesResponseType(typeof(MediaGroupResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateMediaGroup([FromBody] CreateMediaGroupDto dto)
        => Ok(await _service.CreateMediaGroupAsync(dto));
}