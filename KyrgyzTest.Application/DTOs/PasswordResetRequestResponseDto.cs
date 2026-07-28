using KyrgyzTest.Core.Enums;

namespace KyrgyzTest.Application.DTOs;

public record PasswordResetRequestResponseDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string UserFullName { get; set; } = string.Empty;
    public string UserLogin { get; set; } = string.Empty;
    public UserRole UserRole { get; set; }
    public PasswordResetStatus Status { get; set; }
    public DateTime RequestedAt { get; set; }
}
