using KyrgyzTest.Core.Entities;

namespace KyrgyzTest.Application.DTOs;

/// <summary>
/// Live-monitoring row for the admin panel: which candidates are currently
/// testing, their photo (for identity verification), and where they are.
/// </summary>
public class ActiveAttemptDto
{
    public Guid CandidateId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? Photo { get; set; }
    public Guid AttemptId { get; set; }
    public DateTime AttemptStartedAt { get; set; }
    public SectionType? CurrentSection { get; set; }
    public DateTime? SectionDeadlineUtc { get; set; }
}
