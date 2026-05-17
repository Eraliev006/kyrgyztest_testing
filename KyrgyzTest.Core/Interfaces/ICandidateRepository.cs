namespace KyrgyzTest.Core.Interfaces;
using KyrgyzTest.Core.Entities;

public interface ICandidateRepository
{
    Task<Candidate?> GetByIdAsync(Guid id);
    Task<Candidate?> GetByInnAsync(string inn);
    Task<Candidate?> GetByAccessCodeAsync(string accessCode);
    Task<List<Candidate>> GetAllAsync();
    Task<List<Candidate>> SearchByNameAsync(string name);
    Task<(List<Candidate> Items, int TotalCount)> GetPagedAsync(Guid? organizationId, DateTime? dateFrom, DateTime? dateTo, int page, int pageSize);
    Task<Candidate> CreateAsync(Candidate candidate);
    Task<Candidate> UpdateAsync(Candidate candidate);
    Task DeleteAsync(Guid id);
}