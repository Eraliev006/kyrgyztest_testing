using KyrgyzTest.Core.Entities;

namespace KyrgyzTest.Core.Interfaces;

public interface IUserRepository
{
    public Task<Users?> GetById(Guid id);
    public Task<Users> Create(Users user);
    public Task<Users> Update(Users user);
    public Task<Users> Delete(Guid id);
    public Task<List<Users>> GetAll();
    public Task<Users?> GetByLogin(string login);
}