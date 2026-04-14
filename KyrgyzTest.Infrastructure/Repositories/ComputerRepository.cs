using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Enums;
using KyrgyzTest.Core.Interfaces;
using KyrgyzTest.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace KyrgyzTest.Infrastructure.Repositories;

public class ComputerRepository(AppDbContext context) : IComputerRepository
{

    public async Task<Computer?> GetByComputerIdAsync(Guid id)
    {
        return await context.Computers.FirstOrDefaultAsync(c => c.Id == id);
    }
    
    public async Task<Computer?> UpdateComputerStatusAsync(Guid id, ComputerStatus status)
    {
        Computer? computer = await context.Computers.FirstOrDefaultAsync(c => c.Id == id);
        if (computer is not null)
        {
            computer.Status = status;
        }
        await context.SaveChangesAsync();
        return computer;
    }

    public async Task<List<Computer>> GetAllFreeComputersAsync()
    {
        return await context.Computers.Where(c => c.Status == ComputerStatus.Free).ToListAsync();
    }
    
    public async Task<List<Computer>> GetAllComputersAsync()
    {
        return await context.Computers.ToListAsync();
    }
    
    public async Task<Computer?> GetAndReserveFreeComputerAsync()
    {
        var computer = await context.Computers
            .Where(c => c.Status == ComputerStatus.Free)
            .FirstOrDefaultAsync();

        if (computer == null)
            return null;

        computer.Status = ComputerStatus.Occupied;

        await context.SaveChangesAsync();

        return computer;
    }
}