using Microsoft.EntityFrameworkCore;
using Sazzle.Application.Common.Interfaces;
using Sazzle.Domain.Entities;
using Sazzle.Infrastructure.Persistence;

namespace Sazzle.Infrastructure.Repositories;

public class OrganizationRepository : IOrganizationRepository
{
    private readonly SazzleDbContext _context;
    
    public OrganizationRepository(SazzleDbContext context) => _context = context;
    
    public Task<Organization?> GetByIdAsync(Guid id) =>
        _context.Organizations.FirstOrDefaultAsync(o => o.Id == id);
    
    public Task<Organization?> GetBySlugAsync(string slug) =>
        _context.Organizations.FirstOrDefaultAsync(o => o.Slug == slug);
    
    public async Task AddAsync(Organization organization) =>
        await _context.Organizations.AddAsync(organization);

    public Task SaveChangesAsync() => _context.SaveChangesAsync();
    
    
	public async Task<List <Organization>> GetByUserIdAsync(Guid userId) 
	{
		return await _context.Organizations
			.Where(o => o.Members.Any(m => m.UserId == userId))
			.ToListAsync();
	}

	public async Task AddMemberAsync(OrganizationMember member) => await _context.OrganizationMembers.AddAsync(member);


	public async Task<bool> UserHasPermissionAsync(Guid userId, Guid organizationId, string permissionKey)
	{
		return await _context.OrganizationMembers
			.Where(m => m.UserId == userId && m.OrganizationId == organizationId)
			.Join(_context.Roles, m => m.RoleId, r => r.Id, (m, r) => r)
			.SelectMany(r => r.Permissions)
			.AnyAsync(p => p.Key == permissionKey);


	}
}