namespace KyrgyzTest.Application.DTOs;

public record RegisterDto
{
    public string FullName { get; set; }
    public string Role { get; set; }
}