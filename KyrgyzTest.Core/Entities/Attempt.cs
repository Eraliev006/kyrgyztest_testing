using KyrgyzTest.Core.Enums;

namespace KyrgyzTest.Core.Entities;

public class Attempt
{
    public Guid Id { get; set; }
    public Guid CandidateId { get; set; }
    public Candidate Candidate { get; set; } = null!;
    public Guid TestVariantId { get; set; }
    public TestVariant TestVariant { get; set; } = null!;
    public AttemptStatus Status { get; set; }
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime? SubmittedAt { get; set; }
}