using KyrgyzTest.Application.DTOs;

namespace KyrgyzTest.Application.Interfaces;

public interface IUserService
{
    public Task<UserResponseDto?> GetById(Guid id);
    public Task<UserResponseDto?> GetByLogin(string login);
    public Task<List<UserResponseDto>> GetAll();

    public Task<UserResponseDto> Create(CreateUserDto dto);
    public Task<UserResponseDto> Update(Guid id, CreateUserDto dto);
    public Task<UserResponseDto> Delete(Guid id);
}