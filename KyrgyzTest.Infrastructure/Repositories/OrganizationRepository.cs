using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Interfaces;
using KyrgyzTest.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace KyrgyzTest.Infrastructure.Repositories;

public class OrganizationRepository : IOrganizationRepository
{
    private readonly AppDbContext _context;

    public OrganizationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Organization?> GetByIdAsync(Guid id)
        => await _context.Organizations.FirstOrDefaultAsync(o => o.Id == id);

    public async Task<List<Organization>> GetAllAsync()
        => await _context.Organizations.ToListAsync();

    public async Task<Organization> CreateAsync(Organization organization)
    {
        _context.Organizations.Add(organization);
        await _context.SaveChangesAsync();
        return organization;
    }

    public async Task<Organization> UpdateAsync(Organization organization)
    {
        _context.Organizations.Update(organization);
        await _context.SaveChangesAsync();
        return organization;
    }

    public async Task DeleteAsync(Guid id)
    {
        var org = await _context.Organizations.FindAsync(id);
        if (org != null)
        {
            _context.Organizations.Remove(org);
            await _context.SaveChangesAsync();
        }
    }
}
