using Sazzle.Domain.Entities;
namespace Sazzle.Application.Common.Interfaces;


public interface IRoleRepository
{
    Task<IReadOnlyList<Role>> GetSystemRolesAsync();
    Task<Role?> GetSystemRoleByNameAsync(string name); // e.g. "Owner", "Admin", "Member"
    Task AddAsync(Role role);
}
