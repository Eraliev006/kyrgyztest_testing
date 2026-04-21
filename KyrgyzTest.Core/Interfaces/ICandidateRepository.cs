namespace KyrgyzTest.Core.Interfaces;
using KyrgyzTest.Core.Entities;

public interface ICandidateRepository
{
    Task<Candidate?> GetByIdAsync(Guid id);
    Task<Candidate?> GetByInnAsync(string inn);
    Task<Candidate?> GetByAccessCodeAsync(string accessCode);
    Task<List<Candidate>> GetAllAsync();
    Task<Candidate> CreateAsync(Candidate candidate);
    Task<Candidate> UpdateAsync(Candidate candidate);
}