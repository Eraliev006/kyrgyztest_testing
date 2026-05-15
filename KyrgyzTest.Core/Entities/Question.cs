using KyrgyzTest.Core.Enums;

namespace KyrgyzTest.Core.Entities;

public class Question
{
    public Guid Id { get; set; }
    public SectionType Section { get; set; }
    public LanguageLevel Level { get; set; }
    public QuestionType Type { get; set; }
    public string Content { get; set; } = string.Empty;
    public int OrderIndex { get; set; }
    public Guid? MediaGroupId { get; set; }
    public MediaGroup? MediaGroup { get; set; }
    public Guid? TopicId { get; set; }
    public Topic? Topic { get; set; }
    public ICollection<AnswerOption> AnswerOptions { get; set; } = new List<AnswerOption>();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}