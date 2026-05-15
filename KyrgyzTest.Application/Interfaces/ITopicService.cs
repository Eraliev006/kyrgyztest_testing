using KyrgyzTest.Application.DTOs;
using KyrgyzTest.Core.Entities;

namespace KyrgyzTest.Application.Interfaces;

public interface ITopicService
{
    Task<List<TopicDto>> GetAllAsync(SectionType? section);
    Task<TopicDto?> GetByIdAsync(Guid id);
    Task<TopicDto> CreateAsync(CreateTopicDto dto);
    Task<TopicDto> UpdateAsync(Guid id, CreateTopicDto dto);
    Task DeleteAsync(Guid id);
}
