namespace KyrgyzTest.Core.Entities;

public class CompletedSection
{
    public Guid Id { get; set; }
    public Guid AttemptId { get; set; }
    public Attempt Attempt { get; set; } = null!;
    public SectionType Section { get; set; }
    public DateTime CompletedAt { get; set; }
}
