using KyrgyzTest.Core.Enums;

namespace KyrgyzTest.Core.Entities;

public class SessionRegistration
{
    public Guid Id { get; set; }
    public Guid SessionId { get; set; }
    public Guid CandidateId { get; set; }
    public string AccessCode { get; set; } = string.Empty;
    public SessionRegistrationStatus Status { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}