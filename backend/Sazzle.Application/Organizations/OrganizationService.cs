using Sazzle.Application.Common.Interfaces;
using Sazzle.Domain.Entities;


namespace Sazzle.Application.Organizations;

public class OrganizationService
{
    private readonly IOrganizationRepository _organizations;
    private readonly IRoleRepository _roles;

    public OrganizationService(IOrganizationRepository organizations, IRoleRepository roles)
    {
        _organizations = organizations;
        _roles = roles;
    }

    public async Task<Organization> CreateOrganizationAsync(string name, string slug, Guid creatorUserId)
    {
        var existing = await _organizations.GetBySlugAsync(slug);
        if (existing is not null)
            throw new InvalidOperationException($"Slug '{slug}' is already taken.");



        var ownerRole = await _roles.GetSystemRoleByNameAsync("Owner") 
                        ?? throw new InvalidOperationException("System role 'Owner' not seeded.");

        var organization = new Organization(name, slug);
        organization.AddMember(creatorUserId, ownerRole.Id);
        
        await _organizations.AddAsync(organization);
        await _organizations.SaveChangesAsync();

        return organization;
    }
}