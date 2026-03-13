namespace KyrgyzTest.Core.Interfaces;
using KyrgyzTest.Core.Entities;

public interface ICandidateRepository
{
    Task<Candidate?> GetByExamCodeAsync(string examCode);
}