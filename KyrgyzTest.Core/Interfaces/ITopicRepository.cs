using KyrgyzTest.Core.Entities;

namespace KyrgyzTest.Core.Interfaces;

public interface ITopicRepository
{
    Task<List<Topic>> GetAllAsync(SectionType? section);
    Task<Topic?> GetByIdAsync(Guid id);
    Task<Topic> CreateAsync(Topic topic);
    Task<Topic> UpdateAsync(Topic topic);
    Task DeleteAsync(Guid id);
}
