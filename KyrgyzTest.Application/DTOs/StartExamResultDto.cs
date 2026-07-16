using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Enums;

namespace KyrgyzTest.Application.DTOs;

public class StartExamResultDto
{
    public Guid AttemptId { get; set; }
    public List<ExamSectionDto> Sections { get; set; } = new();
}

public class ExamSectionDto
{
    public SectionType Section { get; set; }
    public int TimeLimitMinutes { get; set; }
    public bool IsCompleted { get; set; }
    /// <summary>Null until the candidate actually opens the section (StartSectionAsync).</summary>
    public DateTime? DeadlineUtc { get; set; }
    public List<ExamQuestionDto> Questions { get; set; } = new();
}

public class SectionStatusDto
{
    public bool IsCompleted { get; set; }
    public DateTime? DeadlineUtc { get; set; }
}

public class ExamQuestionDto
{
    public Guid Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public QuestionType Type { get; set; }
    public List<ExamAnswerOptionDto> AnswerOptions { get; set; } = new();
}

public class ExamAnswerOptionDto
{
    public Guid Id { get; set; }
    public string Content { get; set; } = string.Empty;
}
