using Sazzle.Domain.Entities;
using Sazzle.Application.Organizations;


namespace Sazzle.Application.Common.Interfaces;
public interface IOrganizationRepository
{
    Task<Organization?> GetByIdAsync(Guid id);
	Task<List<Organization>> GetByUserIdAsync(Guid userId);
    Task<Organization?> GetBySlugAsync(string slug);
    Task AddAsync(Organization organization);
	Task AddMemberAsync(OrganizationMember member);
	Task<bool> UserHasPermissionAsync(Guid userId, Guid organizationId, string permissionKey);
	

	Task<List<OrganizationMemberDetails>> GetMembersAsync(Guid organizationId);
	Task<bool> RemoveMemberAsync(Guid organizationId, Guid userId);
	Task SaveChangesAsync();
}