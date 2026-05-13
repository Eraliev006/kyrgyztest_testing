using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Enums;
using KyrgyzTest.Core.Interfaces;
using KyrgyzTest.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace KyrgyzTest.Infrastructure.Repositories;

public class QuestionRepository : IQuestionRepository
{
    private readonly AppDbContext _context;

    public QuestionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Question?> GetByIdAsync(Guid id)
        => await _context.Questions
            .Include(q => q.AnswerOptions)
            .Include(q => q.MediaGroup)
            .FirstOrDefaultAsync(q => q.Id == id);

    public async Task<List<Question>> GetAllAsync()
        => await _context.Questions
            .Include(q => q.AnswerOptions)
            .Include(q => q.MediaGroup)
            .ToListAsync();

    public async Task<List<Question>> GetBySectionAsync(SectionType section)
        => await _context.Questions
            .Include(q => q.AnswerOptions)
            .Include(q => q.MediaGroup)
            .Where(q => q.Section == section)
            .ToListAsync();

    public async Task<List<Question>> GetBySectionAndLevelAsync(SectionType section, LanguageLevel level)
        => await _context.Questions
            .Include(q => q.AnswerOptions)
            .Include(q => q.MediaGroup)
            .Where(q => q.Section == section && q.Level == level)
            .ToListAsync();

    public async Task<Question> CreateAsync(Question question)
    {
        _context.Questions.Add(question);
        await _context.SaveChangesAsync();
        return question;
    }

    public async Task<Question> UpdateAsync(Question question)
    {
        // Direct SQL delete — bypasses tracker, avoids conflict with tracked old options
        await _context.AnswerOptions
            .Where(a => a.QuestionId == question.Id)
            .ExecuteDeleteAsync();

        // Detach stale tracked options so SaveChanges doesn't try to act on them
        foreach (var entry in _context.ChangeTracker.Entries<AnswerOption>()
                     .Where(e => e.Entity.QuestionId == question.Id).ToList())
            entry.State = EntityState.Detached;

        _context.AnswerOptions.AddRange(question.AnswerOptions);
        _context.Entry(question).State = EntityState.Modified;

        await _context.SaveChangesAsync();
        return question;
    }

    public async Task DeleteAsync(Guid id)
    {
        var question = await _context.Questions.FindAsync(id);
        if (question != null)
        {
            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();
        }
    }
}