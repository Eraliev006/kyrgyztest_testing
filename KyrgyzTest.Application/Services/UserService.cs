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

    public UserService(IUserRepository repository, IHttpContextAccessor httpContextAccessor)
    {
        _repository = repository;
        _httpContextAccessor = httpContextAccessor;
    }

    private void EnforceRoleCreationPolicy(UserRole targetRole)
    {
        var roleClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;
        if (Enum.TryParse<UserRole>(roleClaim, out var callerRole) && callerRole == UserRole.Director)
        {
            if (targetRole == UserRole.SuperAdmin || targetRole == UserRole.Director)
                throw new BusinessException("Director не может создавать пользователей с ролью SuperAdmin или Director.");
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
        EnforceRoleCreationPolicy(dto.Role);

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
        return Map(created);
    }

    public async Task<UserResponseDto> Update(Guid id, CreateUserDto dto)
    {
        EnforceRoleCreationPolicy(dto.Role);

        var user = await _repository.GetById(id);

        if (user == null)
            throw new Exception("User not found");

        user.FullName = dto.FullName;
        user.Login = dto.Login;
        user.PasswordHash = Hash(dto.Password);
        user.Role = dto.Role;

        var updated = await _repository.Update(user);
        return Map(updated);
    }

    public async Task<UserResponseDto> Delete(Guid id)
    {
        var deleted = await _repository.Delete(id);
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