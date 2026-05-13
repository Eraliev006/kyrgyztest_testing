using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Interfaces;
using KyrgyzTest.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace KyrgyzTest.Infrastructure.Repositories;

public class SectionConfigRepository : ISectionConfigRepository
{
    private readonly AppDbContext _context;

    public SectionConfigRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<SectionConfig?> GetById(Guid id)
    {
        return await _context.SectionConfigs.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<SectionConfig>> GetAll()
    {
        return await _context.SectionConfigs.ToListAsync();
    }

    public async Task<SectionConfig> Create(SectionConfig config)
    {
        _context.SectionConfigs.Add(config);
        await _context.SaveChangesAsync();
        return config;
    }

    public async Task<SectionConfig> Update(SectionConfig config)
    {
        _context.SectionConfigs.Update(config);
        await _context.SaveChangesAsync();
        return config;
    }

    public async Task<SectionConfig> Delete(Guid id)
    {
        var config = await _context.SectionConfigs.FirstOrDefaultAsync(x => x.Id == id);

        if (config == null)
            throw new Exception("SectionConfig not found");

        _context.SectionConfigs.Remove(config);
        await _context.SaveChangesAsync();

        return config;
    }
}
