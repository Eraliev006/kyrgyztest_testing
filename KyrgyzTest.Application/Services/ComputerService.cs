using KyrgyzTest.Application.DTOs;
using KyrgyzTest.Application.Interfaces;
using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Interfaces;

namespace KyrgyzTest.Application.Services;

public class ComputerService: IComputerService
{
    private readonly IComputerRepository _computerRepository;

    public ComputerService(IComputerRepository computerRepository)
    {
        _computerRepository = computerRepository;
    }

    public async Task<List<ComputerResponseDto>> GetAllComputersAsync()
    {
        var computers = await _computerRepository.GetAllComputersAsync();
        return computers.Select(c => new ComputerResponseDto
        {
            StationNumber = c.StationNumber,
            Status = c.Status,
            ExamCode = null
        }).ToList();
        
    }
}