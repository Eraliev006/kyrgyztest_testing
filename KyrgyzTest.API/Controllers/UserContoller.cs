using KyrgyzTest.Application.DTOs;
using KyrgyzTest.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KyrgyzTest.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _service;

    public UserController(IUserService service)
    {
        _service = service;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _service.GetById(id);
        return user == null ? NotFound() : Ok(user);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAll());
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserDto dto)
    {
        var user = await _service.Create(dto);
        return Ok(user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, CreateUserDto dto)
    {
        var user = await _service.Update(id, dto);
        return Ok(user);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var user = await _service.Delete(id);
        return Ok(user);
    }
}