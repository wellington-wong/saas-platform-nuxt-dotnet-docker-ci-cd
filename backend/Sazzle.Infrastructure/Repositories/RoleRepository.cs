using Microsoft.EntityFrameworkCore;
using Sazzle.Application.Common.Interfaces;
using Sazzle.Domain.Entities;
using Sazzle.Infrastructure.Persistence;

namespace Sazzle.Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly SazzleDbContext  _context;
    
    public RoleRepository(SazzleDbContext context) => _context = context;


    public Task<Role?> GetSystemRoleByNameAsync(string name) =>
        _context.Roles.FirstOrDefaultAsync(r => r.OrganizationId == null && r.Name == name);
    
    public async Task AddAsync(Role role) =>
        await _context.Roles.AddAsync(role);
}