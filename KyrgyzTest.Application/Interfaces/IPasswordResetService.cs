using KyrgyzTest.Application.DTOs;

namespace KyrgyzTest.Application.Interfaces;

public interface IPasswordResetService
{
    Task RequestResetAsync(RequestPasswordResetDto dto);
    Task<List<PasswordResetRequestResponseDto>> GetPendingAsync();
    Task ApproveAsync(Guid requestId);
    Task RejectAsync(Guid requestId);
}
