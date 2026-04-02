using KyrgyzTest.Application.DTOs;

namespace KyrgyzTest.Application.Interfaces;

public interface IComputerService
{
    public Task<List<ComputerResponseDto>> GetAllComputersAsync();
}