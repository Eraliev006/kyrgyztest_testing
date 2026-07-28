namespace KyrgyzTest.Core.Entities;

public class SectionTiming
{
    public Guid Id { get; set; }
    public Guid AttemptId { get; set; }
    public Attempt Attempt { get; set; } = null!;
    public SectionType Section { get; set; }
    public DateTime StartedAt { get; set; }
}
