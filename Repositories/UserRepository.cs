using System;
using health_pal_backend.Config;
using health_pal_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace health_pal_backend.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(UserModel user)
    {
        await _context.Users.AddAsync(user);
    }

    public Task UpdateAsync(UserModel user)
    {
        _context.Users.Update(user); // Synchronous operation
        return Task.CompletedTask;  // Return a completed task
    }

    public Task DeleteAsync(UserModel user)
    {
        _context.Users.Remove(user); // Synchronous operation
        return Task.CompletedTask;  // Return a completed task
    }

    public async Task<IEnumerable<UserModel>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<UserModel?> GetByEmailAsync(string email)
    {
        return await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
    }

    public async Task<UserModel?> GetByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
