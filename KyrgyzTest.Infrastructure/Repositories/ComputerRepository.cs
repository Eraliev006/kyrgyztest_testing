using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Enums;
using KyrgyzTest.Core.Interfaces;
using KyrgyzTest.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace KyrgyzTest.Infrastructure.Repositories;

public class ComputerRepository : IComputerRepository
{
    private readonly AppDbContext _context;

    public ComputerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Computer?> GetByStationNumberAsync(int stationNumber)
    {
        return await _context.Computers.FirstOrDefaultAsync(c => c.StationNumber == stationNumber);
    }
    
    public async Task<Computer?> UpdateComputerStatusAsync(Guid id, ComputerStatus status)
    {
        Computer? computer = await _context.Computers.FirstOrDefaultAsync(c => c.Id == id);
        if (computer is not null)
        {
            computer.Status = status;
        }
        await _context.SaveChangesAsync();
        return computer;
    }

    public async Task<List<Computer>> GetAllFreeComputersAsync()
    {
        return await _context.Computers.Where(c => c.Status == ComputerStatus.Free).ToListAsync();
    }
    
    public async Task<List<Computer>> GetAllComputersAsync()
    {
        return await _context.Computers.ToListAsync();
    }
}