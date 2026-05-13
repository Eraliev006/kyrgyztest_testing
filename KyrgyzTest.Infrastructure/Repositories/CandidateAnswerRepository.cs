using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Interfaces;
using KyrgyzTest.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace KyrgyzTest.Infrastructure.Repositories;

public class CandidateAnswerRepository : ICandidateAnswerRepository
{
    private readonly AppDbContext _context;

    public CandidateAnswerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CandidateAnswer?> GetByAttemptAndQuestionAsync(Guid attemptId, Guid questionId)
        => await _context.CandidateAnswers
            .FirstOrDefaultAsync(a => a.AttemptId == attemptId && a.QuestionId == questionId);

    public async Task<List<CandidateAnswer>> GetByAttemptIdAsync(Guid attemptId)
        => await _context.CandidateAnswers
            .Where(a => a.AttemptId == attemptId)
            .ToListAsync();

    public async Task UpsertAsync(CandidateAnswer answer)
    {
        var existing = await GetByAttemptAndQuestionAsync(answer.AttemptId, answer.QuestionId);
        if (existing == null)
        {
            _context.CandidateAnswers.Add(answer);
        }
        else
        {
            existing.SelectedOptionId = answer.SelectedOptionId;
            existing.OrderedAnswer = answer.OrderedAnswer;
            _context.CandidateAnswers.Update(existing);
        }
        await _context.SaveChangesAsync();
    }
}
