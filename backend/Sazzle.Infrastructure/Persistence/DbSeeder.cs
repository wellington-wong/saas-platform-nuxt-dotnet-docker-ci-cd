using Microsoft.EntityFrameworkCore;
using Sazzle.Domain.Entities;
namespace Sazzle.Infrastructure.Persistence;

public static class DbSeeder
{
    public static async Task SeedSystemRolesAsync(SazzleDbContext context)
    {
        var roles = await context.Roles
            .Include(role => role.Permissions)
            .Where(role => role.OrganizationId == null)
            .ToListAsync();

        var owner = roles.FirstOrDefault(role => role.Name == "Owner");
        var admin = roles.FirstOrDefault(role => role.Name == "Admin");
        var member = roles.FirstOrDefault(role => role.Name == "Member");

        var invite = await GetOrAddPermissionAsync(context, "members:invite", "Invite new members to the organization");
        var remove = await GetOrAddPermissionAsync(context, "members:remove", "Remove members from the organization");
        var manageRoles = await GetOrAddPermissionAsync(context, "roles:manage", "Create and assign custom roles");
        var manageTeams = await GetOrAddPermissionAsync(context, "teams:manage", "Create and manage organization teams");
        var billing = await GetOrAddPermissionAsync(context, "billing:write", "Manage billing and subscription");
        var view = await GetOrAddPermissionAsync(context, "members:view", "View organization members");

        owner ??= new Role("Owner");
        admin ??= new Role("Admin");
        member ??= new Role("Member");

        owner.GrantPermission(invite);
        owner.GrantPermission(remove);
        owner.GrantPermission(manageRoles);
        owner.GrantPermission(manageTeams);
        owner.GrantPermission(billing);
        owner.GrantPermission(view);

        admin.GrantPermission(invite);
        admin.GrantPermission(remove);
        admin.GrantPermission(view);

        member.GrantPermission(view);

        context.Roles.AddRange(new[] { owner, admin, member }.Where(role => !roles.Contains(role)));
        await context.SaveChangesAsync();
    }

    private static async Task<Permission> GetOrAddPermissionAsync(
        SazzleDbContext context, string key, string description)
    {
        var permission = await context.Permissions.FirstOrDefaultAsync(item => item.Key == key);
        if (permission is not null)
            return permission;

        permission = new Permission(key, description);
        context.Permissions.Add(permission);
        return permission;
    }
}
