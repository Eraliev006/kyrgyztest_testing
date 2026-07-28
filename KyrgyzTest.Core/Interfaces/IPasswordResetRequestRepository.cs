using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Enums;

namespace KyrgyzTest.Core.Interfaces;

public interface IPasswordResetRequestRepository
{
    Task<PasswordResetRequest?> GetByIdAsync(Guid id);
    Task<PasswordResetRequest?> GetPendingByUserIdAsync(Guid userId);
    Task<List<PasswordResetRequest>> GetByStatusAsync(PasswordResetStatus status);
    Task<PasswordResetRequest> CreateAsync(PasswordResetRequest request);
    Task UpdateAsync(PasswordResetRequest request);
}
