using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Interfaces;
using KyrgyzTest.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace KyrgyzTest.Infrastructure.Repositories;

public class MediaGroupRepository : IMediaGroupRepository
{
    private readonly AppDbContext _context;

    public MediaGroupRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<MediaGroup?> GetByIdAsync(Guid id)
        => await _context.MediaGroups
            .Include(m => m.Questions)
            .FirstOrDefaultAsync(m => m.Id == id);

    public async Task<MediaGroup> CreateAsync(MediaGroup mediaGroup)
    {
        _context.MediaGroups.Add(mediaGroup);
        await _context.SaveChangesAsync();
        return mediaGroup;
    }

    public async Task DeleteAsync(Guid id)
    {
        var mediaGroup = await _context.MediaGroups.FindAsync(id);
        if (mediaGroup != null)
        {
            _context.MediaGroups.Remove(mediaGroup);
            await _context.SaveChangesAsync();
        }
    }
}