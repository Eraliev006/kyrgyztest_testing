namespace KyrgyzTest.Application.DTOs;

public record RequestPasswordResetDto
{
    public string Login { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}
