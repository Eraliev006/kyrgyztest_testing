namespace KyrgyzTest.Application.DTOs;

public record LoginResponseDto
{
    public string AccessToken { get; set; }
    public string FullName { get; set; }
    public string Login  { get; set; }
    public string Role { get; set; }
};