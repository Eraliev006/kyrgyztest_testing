namespace KyrgyzTest.Core.Interfaces;
using KyrgyzTest.Core.Entities;

public interface ICandidateRepository
{
    Task<Candidate?> GetByExamCodeAsync(string examCode);
    Task<Candidate> CreateAsync(Candidate candidate);
}