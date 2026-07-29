using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Interfaces;
using KyrgyzTest.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace KyrgyzTest.Infrastructure.Repositories;

public class ExamAccessSettingsRepository : IExamAccessSettingsRepository
{
    private readonly AppDbContext _context;

    public ExamAccessSettingsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ExamAccessSettings?> GetAsync()
    {
        return await _context.ExamAccessSettings.Include(s => s.UpdatedBy).FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(ExamAccessSettings settings)
    {
        _context.ExamAccessSettings.Update(settings);
        await _context.SaveChangesAsync();
    }
}
