using KyrgyzTest.Core.Enums;
using KyrgyzTest.Core.Entities;

namespace KyrgyzTest.Core.Interfaces;


public interface IComputerRepository
{
    public Task<Computer?> GetByComputerIdAsync(Guid id);
    public Task<List<Computer>> GetAllFreeComputersAsync();
    public Task<Computer?> UpdateComputerStatusAsync(Guid id, ComputerStatus status);
    public Task<List<Computer>> GetAllComputersAsync();
    public Task<Computer?> GetAndReserveFreeComputerAsync();
    
    public Task<Computer?> GetByDeviceIdAsync(string deviceId);
    public Task<Computer> RegisterPendingComputerAsync(Computer computer);
    public Task<Computer?> ApproveComputerAsync(Guid id, int stationNumber);
}