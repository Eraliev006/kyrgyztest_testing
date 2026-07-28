using KyrgyzTest.Core.Entities;

namespace KyrgyzTest.Core.Interfaces;

public interface IManualGradeRepository
{
    Task<ManualGrade?> GetByAttemptAndQuestionAsync(Guid attemptId, Guid questionId);
    Task<List<ManualGrade>> GetByAttemptIdAsync(Guid attemptId);
    Task UpsertAsync(ManualGrade grade);
}
