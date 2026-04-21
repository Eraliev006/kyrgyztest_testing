using KyrgyzTest.Core.Enums;

namespace KyrgyzTest.Application.DTOs;

public record CreateMediaGroupDto(
    MediaType Type,
    string Content
);