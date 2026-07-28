using KyrgyzTest.Application.DTOs;

namespace KyrgyzTest.Application.Interfaces;

public interface IAuthService
{
    public Task<LoginResponseDto> LoginUser(LoginDto dto);
    public Task ChangePasswordAsync(Guid userId, ChangePasswordDto dto);
}