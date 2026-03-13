using KyrgyzTest.Core.Entities;

namespace KyrgyzTest.Core.Interfaces;

public interface IExamSessionRepository
{
    public Task<ExamSession?> GetByExamCodeAsync(string examSessionId); 
    public Task<ExamSession?> CreateNewExamSessionAsync(ExamSession examSession);
    public Task<ExamSession?> UpdateSessionAsync(ExamSession examSession);
}