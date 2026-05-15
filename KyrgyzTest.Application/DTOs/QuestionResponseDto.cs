using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Enums;

namespace KyrgyzTest.Application.DTOs;

public record QuestionResponseDto
{
    public Guid Id { get; set; }
    public SectionType Section { get; set; }
    public LanguageLevel Level { get; set; }
    public QuestionType Type { get; set; }
    public string Content { get; set; } = string.Empty;
    public Guid? MediaGroupId { get; set; }
    public string? MediaUrl { get; set; }
    public string? MediaText { get; set; }
    public Guid? TopicId { get; set; }
    public string? TopicName { get; set; }
    public List<AnswerOptionResponseDto> AnswerOptions { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}

public record AnswerOptionResponseDto
{
    public Guid Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
    public int OrderIndex { get; set; }
}