using KyrgyzTest.Core.Enums;

namespace KyrgyzTest.Core.Entities;

public class Users
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Login { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}