using KyrgyzTest.Application.DTOs;
using KyrgyzTest.Application.Interfaces;
using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Exceptions;
using KyrgyzTest.Core.Interfaces;

namespace KyrgyzTest.Application.Services;

public class TopicService : ITopicService
{
    private readonly ITopicRepository _topicRepository;

    public TopicService(ITopicRepository topicRepository)
    {
        _topicRepository = topicRepository;
    }

    public async Task<List<TopicDto>> GetAllAsync(SectionType? section)
    {
        var topics = await _topicRepository.GetAllAsync(section);
        return topics.Select(Map).ToList();
    }

    public async Task<TopicDto?> GetByIdAsync(Guid id)
    {
        var topic = await _topicRepository.GetByIdAsync(id);
        return topic == null ? null : Map(topic);
    }

    public async Task<TopicDto> CreateAsync(CreateTopicDto dto)
    {
        var topic = new Topic
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            SectionType = dto.SectionType
        };
        var created = await _topicRepository.CreateAsync(topic);
        return Map(created);
    }

    public async Task<TopicDto> UpdateAsync(Guid id, CreateTopicDto dto)
    {
        var topic = await _topicRepository.GetByIdAsync(id)
                    ?? throw new NotFoundException("Тема не найдена");
        topic.Name = dto.Name;
        topic.SectionType = dto.SectionType;
        var updated = await _topicRepository.UpdateAsync(topic);
        return Map(updated);
    }

    public async Task DeleteAsync(Guid id)
    {
        var topic = await _topicRepository.GetByIdAsync(id)
                    ?? throw new NotFoundException("Тема не найдена");
        await _topicRepository.DeleteAsync(topic.Id);
    }

    private static TopicDto Map(Topic t) => new(t.Id, t.Name, t.SectionType);
}
