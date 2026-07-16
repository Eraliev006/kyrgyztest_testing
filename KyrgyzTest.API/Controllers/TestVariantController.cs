using KyrgyzTest.Application.DTOs;
using KyrgyzTest.Application.Interfaces;
using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KyrgyzTest.API.Controllers;

[ApiController]
[Route("api/variants")]
[Authorize(Roles = "SuperAdmin,Director")]
public class TestVariantController : ControllerBase
{
    private readonly ITestVariantService _service;

    public TestVariantController(ITestVariantService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<TestVariantSummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
        => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TestVariantDetailDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid id)
        => Ok(await _service.GetByIdAsync(id));

    [HttpGet("{id}/available-questions")]
    [ProducesResponseType(typeof(List<VariantQuestionDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAvailableQuestions(Guid id, [FromQuery] SectionType section, [FromQuery] LanguageLevel level)
        => Ok(await _service.GetAvailableQuestionsAsync(id, section, level));

    [HttpPut("{id}/questions/{questionId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ReplaceQuestion(Guid id, Guid questionId, [FromBody] ReplaceQuestionDto dto)
    {
        await _service.ReplaceQuestionAsync(id, questionId, dto.NewQuestionId);
        return NoContent();
    }
}
