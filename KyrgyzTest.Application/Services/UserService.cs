using System.Security.Cryptography;
using System.Text;
using KyrgyzTest.Application.DTOs;
using KyrgyzTest.Application.Interfaces;
using KyrgyzTest.Core.Entities;
using KyrgyzTest.Core.Interfaces;

namespace KyrgyzTest.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository)
    {
        _repository = repository;
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
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes);
    }
}