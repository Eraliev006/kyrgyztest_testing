namespace KyrgyzTest.Core.Entities;

public class CandidateAnswer
{
    public Guid Id { get; set; }
    public Guid AttemptId { get; set; }
    public Attempt Attempt { get; set; } = null!;
    public Guid QuestionId { get; set; }
    public Question Question { get; set; } = null!;
    public Guid? SelectedOptionId { get; set; }
    public string? OrderedAnswer { get; set; }
}