using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Enums;

namespace KyrgyzTest.Application.DTOs;

public class AttemptDetailDto
{
    public Guid AttemptId { get; set; }
    public Guid CandidateId { get; set; }
    public List<AttemptAnswerDetailDto> Answers { get; set; } = new();
}

public class AttemptAnswerDetailDto
{
    public Guid QuestionId { get; set; }
    public string Content { get; set; } = string.Empty;
    public SectionType Section { get; set; }
    public LanguageLevel Level { get; set; }
    public QuestionType Type { get; set; }
    public Guid? SelectedOptionId { get; set; }
    public string? OrderedAnswer { get; set; }
    public string? AudioAnswerUrl { get; set; }
    public int? ManualScore { get; set; }
    public bool IsCorrect { get; set; }
    public Guid? CorrectOptionId { get; set; }
    public Guid[]? CorrectOrder { get; set; }
}
