using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Enums;
using KyrgyzTest.Core.Interfaces;
using KyrgyzTest.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace KyrgyzTest.Infrastructure.Repositories;

public class PasswordResetRequestRepository : IPasswordResetRequestRepository
{
    private readonly AppDbContext _context;

    public PasswordResetRequestRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PasswordResetRequest?> GetByIdAsync(Guid id)
    {
        return await _context.PasswordResetRequests
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<PasswordResetRequest?> GetPendingByUserIdAsync(Guid userId)
    {
        return await _context.PasswordResetRequests
            .FirstOrDefaultAsync(r => r.UserId == userId && r.Status == PasswordResetStatus.Pending);
    }

    public async Task<List<PasswordResetRequest>> GetByStatusAsync(PasswordResetStatus status)
    {
        return await _context.PasswordResetRequests
            .Include(r => r.User)
            .Where(r => r.Status == status)
            .OrderBy(r => r.RequestedAt)
            .ToListAsync();
    }

    public async Task<PasswordResetRequest> CreateAsync(PasswordResetRequest request)
    {
        _context.PasswordResetRequests.Add(request);
        await _context.SaveChangesAsync();
        return request;
    }

    public async Task UpdateAsync(PasswordResetRequest request)
    {
        _context.PasswordResetRequests.Update(request);
        await _context.SaveChangesAsync();
    }
}
