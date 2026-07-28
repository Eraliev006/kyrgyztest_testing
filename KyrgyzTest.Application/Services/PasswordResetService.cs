using System.Security.Claims;
using KyrgyzTest.Application.DTOs;
using KyrgyzTest.Application.Interfaces;
using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Enums;
using KyrgyzTest.Core.Exceptions;
using KyrgyzTest.Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace KyrgyzTest.Application.Services;

public class PasswordResetService : IPasswordResetService
{
    private readonly IPasswordResetRequestRepository _repository;
    private readonly IUserRepository _userRepository;
    private readonly IAuditService _audit;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PasswordResetService(
        IPasswordResetRequestRepository repository,
        IUserRepository userRepository,
        IAuditService audit,
        IHttpContextAccessor httpContextAccessor)
    {
        _repository = repository;
        _userRepository = userRepository;
        _audit = audit;
        _httpContextAccessor = httpContextAccessor;
    }

    private void EnforceRoleAccessPolicy(UserRole targetRole)
    {
        var roleClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;
        if (Enum.TryParse<UserRole>(roleClaim, out var callerRole) && callerRole == UserRole.Director)
        {
            if (targetRole == UserRole.SuperAdmin || targetRole == UserRole.Director)
                throw new BusinessException("Director не может управлять пользователями с ролью SuperAdmin или Director.");
        }
    }

    private Guid CurrentUserId()
    {
        var idClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(idClaim!);
    }

    public async Task RequestResetAsync(RequestPasswordResetDto dto)
    {
        var user = await _userRepository.GetByLogin(dto.Login)
            ?? throw new NotFoundException("Пользователь не найден");

        var existing = await _repository.GetPendingByUserIdAsync(user.Id);
        var newHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

        if (existing != null)
        {
            existing.NewPasswordHash = newHash;
            existing.RequestedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(existing);
            return;
        }

        await _repository.CreateAsync(new PasswordResetRequest
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            NewPasswordHash = newHash,
            Status = PasswordResetStatus.Pending,
            RequestedAt = DateTime.UtcNow
        });
    }

    public async Task<List<PasswordResetRequestResponseDto>> GetPendingAsync()
    {
        var requests = await _repository.GetByStatusAsync(PasswordResetStatus.Pending);
        return requests.Select(r => new PasswordResetRequestResponseDto
        {
            Id = r.Id,
            UserId = r.UserId,
            UserFullName = r.User.FullName,
            UserLogin = r.User.Login,
            UserRole = r.User.Role,
            Status = r.Status,
            RequestedAt = r.RequestedAt
        }).ToList();
    }

    public async Task ApproveAsync(Guid requestId)
    {
        var request = await _repository.GetByIdAsync(requestId)
            ?? throw new NotFoundException("Заявка не найдена");

        if (request.Status != PasswordResetStatus.Pending)
            throw new BusinessException("Заявка уже обработана");

        EnforceRoleAccessPolicy(request.User.Role);

        request.User.PasswordHash = request.NewPasswordHash;
        await _userRepository.Update(request.User);

        request.Status = PasswordResetStatus.Approved;
        request.ReviewedAt = DateTime.UtcNow;
        request.ReviewedByUserId = CurrentUserId();
        await _repository.UpdateAsync(request);

        await _audit.LogAsync("APPROVE", "PasswordResetRequest", request.Id,
            $"Одобрен сброс пароля для {request.User.FullName} ({request.User.Login})");
    }

    public async Task RejectAsync(Guid requestId)
    {
        var request = await _repository.GetByIdAsync(requestId)
            ?? throw new NotFoundException("Заявка не найдена");

        if (request.Status != PasswordResetStatus.Pending)
            throw new BusinessException("Заявка уже обработана");

        EnforceRoleAccessPolicy(request.User.Role);

        request.Status = PasswordResetStatus.Rejected;
        request.ReviewedAt = DateTime.UtcNow;
        request.ReviewedByUserId = CurrentUserId();
        await _repository.UpdateAsync(request);

        await _audit.LogAsync("REJECT", "PasswordResetRequest", request.Id,
            $"Отклонён сброс пароля для {request.User.FullName} ({request.User.Login})");
    }
}
