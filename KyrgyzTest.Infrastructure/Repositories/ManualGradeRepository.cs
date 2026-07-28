using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Interfaces;
using KyrgyzTest.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace KyrgyzTest.Infrastructure.Repositories;

public class ManualGradeRepository : IManualGradeRepository
{
    private readonly AppDbContext _context;

    public ManualGradeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ManualGrade?> GetByAttemptAndQuestionAsync(Guid attemptId, Guid questionId)
        => await _context.ManualGrades
            .FirstOrDefaultAsync(g => g.AttemptId == attemptId && g.QuestionId == questionId);

    public async Task<List<ManualGrade>> GetByAttemptIdAsync(Guid attemptId)
        => await _context.ManualGrades
            .Where(g => g.AttemptId == attemptId)
            .ToListAsync();

    public async Task UpsertAsync(ManualGrade grade)
    {
        var existing = await GetByAttemptAndQuestionAsync(grade.AttemptId, grade.QuestionId);
        if (existing == null)
        {
            _context.ManualGrades.Add(grade);
        }
        else
        {
            existing.Score = grade.Score;
            existing.GradedByUserId = grade.GradedByUserId;
            existing.GradedAt = grade.GradedAt;
            _context.ManualGrades.Update(existing);
        }
        await _context.SaveChangesAsync();
    }
}
