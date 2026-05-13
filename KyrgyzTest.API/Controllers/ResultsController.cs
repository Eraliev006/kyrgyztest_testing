using KyrgyzTest.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KyrgyzTest.API.Controllers;

[ApiController]
[Route("api/results")]
[Authorize]
public class ResultsController : ControllerBase
{
    private readonly IResultRepository _resultRepository;

    public ResultsController(IResultRepository resultRepository)
    {
        _resultRepository = resultRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var results = await _resultRepository.GetAllAsync();
        return Ok(results);
    }

    [HttpGet("{candidateId:guid}")]
    public async Task<IActionResult> GetByCandidate(Guid candidateId)
    {
        var results = await _resultRepository.GetByCandidateIdAsync(candidateId);
        return Ok(results);
    }
}
