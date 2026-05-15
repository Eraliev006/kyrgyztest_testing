using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Interfaces;
using KyrgyzTest.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace KyrgyzTest.Infrastructure.Repositories;

public class TestVariantRepository : ITestVariantRepository
{
    private readonly AppDbContext _context;

    public TestVariantRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TestVariant?> GetByIdAsync(Guid id)
        => await _context.TestVariants
            .Include(v => v.Questions)
                .ThenInclude(vq => vq.Question)
                    .ThenInclude(q => q.AnswerOptions)
            .FirstOrDefaultAsync(v => v.Id == id);

    public async Task<List<TestVariant>> GetAllAsync()
        => await _context.TestVariants
            .Include(v => v.Questions)
            .ToListAsync();

    public async Task<int> GetMaxNumberAsync()
        => await _context.TestVariants.AnyAsync()
            ? await _context.TestVariants.MaxAsync(v => v.Number)
            : 0;

    public async Task AddAsync(TestVariant variant)
    {
        _context.TestVariants.Add(variant);
        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync()
        => await _context.SaveChangesAsync();
}
