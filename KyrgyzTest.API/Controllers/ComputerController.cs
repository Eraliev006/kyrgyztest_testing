using KyrgyzTest.Application.DTOs;
using KyrgyzTest.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KyrgyzTest.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ComputerController: ControllerBase
{
    private readonly IComputerService _computerService;

    public ComputerController(IComputerService computerService)
    {
        _computerService = computerService;
    }
    [HttpGet]
    public async Task<ActionResult<List<ComputerResponseDto>>> GetAllComputersAsync()
    {
        var result = await _computerService.GetAllComputersAsync();
        return Ok(result);
    }
}