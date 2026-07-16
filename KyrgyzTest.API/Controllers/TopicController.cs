using KyrgyzTest.Application.DTOs;
using KyrgyzTest.Application.Interfaces;
using KyrgyzTest.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KyrgyzTest.API.Controllers;

[ApiController]
[Route("api/topics")]
[Authorize(Roles = "SuperAdmin,Director,Expert")]
public class TopicController : ControllerBase
{
    private readonly ITopicService _service;

    public TopicController(ITopicService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<TopicDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] SectionType? section)
        => Ok(await _service.GetAllAsync(section));

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TopicDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var topic = await _service.GetByIdAsync(id);
        return topic == null ? NotFound() : Ok(topic);
    }

    [HttpPost]
    [ProducesResponseType(typeof(TopicDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Create([FromBody] CreateTopicDto dto)
        => Ok(await _service.CreateAsync(dto));

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(TopicDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(Guid id, [FromBody] CreateTopicDto dto)
        => Ok(await _service.UpdateAsync(id, dto));

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
