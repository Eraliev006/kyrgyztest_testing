namespace KyrgyzTest.Core.Entities;

public class ManualGrade
{
    public Guid Id { get; set; }
    public Guid AttemptId { get; set; }
    public Attempt Attempt { get; set; } = null!;
    public Guid QuestionId { get; set; }
    public Question Question { get; set; } = null!;
    public int Score { get; set; }
    public Guid GradedByUserId { get; set; }
    public Users GradedBy { get; set; } = null!;
    public DateTime GradedAt { get; set; }
}
