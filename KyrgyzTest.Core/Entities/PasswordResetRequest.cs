using KyrgyzTest.Core.Enums;

namespace KyrgyzTest.Core.Entities;

public class PasswordResetRequest
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Users User { get; set; } = null!;
    public string NewPasswordHash { get; set; } = string.Empty;
    public PasswordResetStatus Status { get; set; } = PasswordResetStatus.Pending;
    public DateTime RequestedAt { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public Guid? ReviewedByUserId { get; set; }
    public Users? ReviewedBy { get; set; }
}
