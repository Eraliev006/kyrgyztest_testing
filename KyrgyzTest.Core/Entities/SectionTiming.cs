namespace KyrgyzTest.Core.Entities;

/// <summary>
/// Persists when a candidate first started a section, so the countdown survives
/// a refresh — StartSectionAsync is idempotent and must return the same
/// StartedAt on every call for a given (AttemptId, Section), not reset it.
/// </summary>
public class SectionTiming
{
    public Guid Id { get; set; }
    public Guid AttemptId { get; set; }
    public Attempt Attempt { get; set; } = null!;
    public SectionType Section { get; set; }
    public DateTime StartedAt { get; set; }
}
