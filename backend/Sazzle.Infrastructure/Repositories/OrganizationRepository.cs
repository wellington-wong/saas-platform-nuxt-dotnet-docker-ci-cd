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
    
    
}