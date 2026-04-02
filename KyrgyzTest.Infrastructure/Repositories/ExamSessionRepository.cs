using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Interfaces;
using KyrgyzTest.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace KyrgyzTest.Infrastructure.Repositories;

public class ExamSessionRepository : IExamSessionRepository
{
    private readonly AppDbContext _context;

    public ExamSessionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ExamSession?> CreateNewExamSessionAsync(ExamSession examSession)
    {
        _context.ExamSessions.Add(examSession);
        await  _context.SaveChangesAsync();
        return examSession;
    }

    public async Task<ExamSession?> GetByExamCodeAsync(string examSessionId)
    {
        return await _context.ExamSessions.FirstOrDefaultAsync(e => e.ExamCode == examSessionId);
    }

    public async Task<ExamSession?> UpdateSessionAsync(ExamSession examSession)
    {
        examSession = _context.ExamSessions.Update(examSession).Entity;
        await _context.SaveChangesAsync();
        return examSession;
    }
}