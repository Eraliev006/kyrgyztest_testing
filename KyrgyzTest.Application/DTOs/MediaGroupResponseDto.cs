using KyrgyzTest.Core.Enums;

namespace KyrgyzTest.Application.DTOs;

public record MediaGroupResponseDto
{
    public Guid Id { get; set; }
    public MediaType Type { get; set; }
    public string Content { get; set; } = string.Empty;
}