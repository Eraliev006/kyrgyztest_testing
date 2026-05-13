using KyrgyzTest.Core.Entities;

namespace KyrgyzTest.Core.Interfaces;

public interface ICandidateAnswerRepository
{
    Task<CandidateAnswer?> GetByAttemptAndQuestionAsync(Guid attemptId, Guid questionId);
    Task<List<CandidateAnswer>> GetByAttemptIdAsync(Guid attemptId);
    Task UpsertAsync(CandidateAnswer answer);
}
