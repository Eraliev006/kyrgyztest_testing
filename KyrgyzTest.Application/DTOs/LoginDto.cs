namespace KyrgyzTest.Application.DTOs;

public record LoginDto
{
    public string Login { get; set; }
    public string Password { get; set; }
}