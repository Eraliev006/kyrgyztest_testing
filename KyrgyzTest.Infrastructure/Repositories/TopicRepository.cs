using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Interfaces;
using KyrgyzTest.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace KyrgyzTest.Infrastructure.Repositories;

public class TopicRepository : ITopicRepository
{
    private readonly AppDbContext _context;

    public TopicRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Topic>> GetAllAsync(SectionType? section)
    {
        var query = _context.Topics.AsQueryable();
        if (section.HasValue)
            query = query.Where(t => t.SectionType == null || t.SectionType == section.Value);
        return await query.ToListAsync();
    }

    public async Task<Topic?> GetByIdAsync(Guid id)
        => await _context.Topics.FirstOrDefaultAsync(t => t.Id == id);

    public async Task<Topic> CreateAsync(Topic topic)
    {
        _context.Topics.Add(topic);
        await _context.SaveChangesAsync();
        return topic;
    }

    public async Task<Topic> UpdateAsync(Topic topic)
    {
        _context.Topics.Update(topic);
        await _context.SaveChangesAsync();
        return topic;
    }

    public async Task DeleteAsync(Guid id)
    {
        var topic = await _context.Topics.FindAsync(id);
        if (topic != null)
        {
            _context.Topics.Remove(topic);
            await _context.SaveChangesAsync();
        }
    }
}
