using KyrgyzTest.API.Hubs;
using KyrgyzTest.Application.DTOs;
using KyrgyzTest.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace KyrgyzTest.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ComputerController: ControllerBase
{
    private readonly IComputerService _computerService;
    private readonly IHubContext<StationHub> _hubContext;

    public ComputerController(IComputerService computerService, IHubContext<StationHub> hubContext)
    {
        _computerService = computerService;
        _hubContext = hubContext;
    }
    [HttpGet]
    public async Task<ActionResult<List<ComputerResponseDto>>> GetAllComputersAsync()
    {
        var result = await _computerService.GetAllComputersAsync();
        return Ok(result);
    }
    
    [HttpPost("open-exam")]
    public async Task<ActionResult> OpenExam(int stationNumber, string examCode)
    {
        await _hubContext.Clients
            .Group($"station-{stationNumber}")
            .SendAsync("ExamOpened", examCode);

        return Ok();
    }
}