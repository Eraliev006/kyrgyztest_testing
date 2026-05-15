using KyrgyzTest.Core.Entities;

namespace KyrgyzTest.Application.DTOs;

public record TopicDto(Guid Id, string Name, SectionType? SectionType);

public record CreateTopicDto(string Name, SectionType? SectionType);
