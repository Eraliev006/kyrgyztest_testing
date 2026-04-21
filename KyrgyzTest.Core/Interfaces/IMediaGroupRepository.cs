using KyrgyzTest.Core.Entities;

namespace KyrgyzTest.Core.Interfaces;

public interface IMediaGroupRepository
{
    Task<MediaGroup?> GetByIdAsync(Guid id);
    Task<MediaGroup> CreateAsync(MediaGroup mediaGroup);
    Task DeleteAsync(Guid id);
}