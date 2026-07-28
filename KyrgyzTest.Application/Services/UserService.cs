using System.Security.Claims;
using KyrgyzTest.Application.DTOs;
using KyrgyzTest.Application.Interfaces;
using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Enums;
using KyrgyzTest.Core.Exceptions;
using KyrgyzTest.Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace KyrgyzTest.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAuditService _audit;

    public UserService(IUserRepository repository, IHttpContextAccessor httpContextAccessor, IAuditService audit)
    {
        _repository = repository;
        _httpContextAccessor = httpContextAccessor;
        _audit = audit;
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

    public async Task<UserResponseDto?> GetById(Guid id)
    {
        var user = await _repository.GetById(id);
        return user == null ? null : Map(user);
    }

    public async Task<UserResponseDto?> GetByLogin(string login)
    {
        var user = await _repository.GetByLogin(login);
        return user == null ? null : Map(user);
    }

    public async Task<List<UserResponseDto>> GetAll()
    {
        var users = await _repository.GetAll();
        return users.Select(Map).ToList();
    }

    public async Task<UserResponseDto> Create(CreateUserDto dto)
    {
        EnforceRoleAccessPolicy(dto.Role);

        var user = new Users
        {
            Id = Guid.NewGuid(),
            FullName = dto.FullName,
            Login = dto.Login,
            PasswordHash = Hash(dto.Password),
            Role = dto.Role,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _repository.Create(user);
        await _audit.LogAsync("CREATE", "User", created.Id, $"Создан сотрудник {created.FullName} ({created.Login}), роль {created.Role}");
        return Map(created);
    }

    public async Task<UserResponseDto> Update(Guid id, CreateUserDto dto)
    {
        EnforceRoleAccessPolicy(dto.Role);

        var user = await _repository.GetById(id)
            ?? throw new NotFoundException("Пользователь не найден");

        user.FullName = dto.FullName;
        user.Login = dto.Login;
        if (!string.IsNullOrWhiteSpace(dto.Password))
            user.PasswordHash = Hash(dto.Password);
        user.Role = dto.Role;

        var updated = await _repository.Update(user);
        await _audit.LogAsync("UPDATE", "User", updated.Id, $"Обновлён сотрудник {updated.FullName} ({updated.Login})");
        return Map(updated);
    }

    public async Task<UserResponseDto> Delete(Guid id)
    {
        var user = await _repository.GetById(id)
            ?? throw new NotFoundException("Пользователь не найден");

        EnforceRoleAccessPolicy(user.Role);

        var deleted = await _repository.Delete(id);
        await _audit.LogAsync("DELETE", "User", deleted.Id, $"Удалён сотрудник {deleted.FullName} ({deleted.Login})");
        return Map(deleted);
    }

    private static UserResponseDto Map(Users user)
    {
        return new UserResponseDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Login = user.Login,
            Role = user.Role,
            CreatedAt = user.CreatedAt
        };
    }

    private static string Hash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}