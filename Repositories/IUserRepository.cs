using System;
using health_pal_backend.Models;

namespace health_pal_backend.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<UserModel>> GetAllAsync();
    Task<UserModel?> GetByIdAsync(int id);
    Task<UserModel?> GetByEmailAsync(string email);
    Task AddAsync(UserModel user);
    Task UpdateAsync(UserModel user);
    Task DeleteAsync(UserModel user);
    Task SaveChangesAsync();
}
