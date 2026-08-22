using Microsoft.EntityFrameworkCore;
using Sazzle.Application.Common.Interfaces;
using Sazzle.Domain.Entities;
using Sazzle.Infrastructure.Persistence;

namespace Sazzle.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly SazzleDbContext _context;
    
    public UserRepository(SazzleDbContext context) => _context = context;
    
    public Task<User?> GetByIdAsync(Guid id) =>
        _context.Users.FirstOrDefaultAsync(u => u.Id == id);

    public Task<User?> GetByEmailAsync(string email) =>
        _context.Users.FirstOrDefaultAsync(u => u.Email == email.ToLowerInvariant().Trim());
    
    public async Task AddAsync(User user) =>
        await _context.Users.AddAsync(user);

    public Task SaveChangesAsync() => _context.SaveChangesAsync();
}