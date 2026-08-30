using Microsoft.EntityFrameworkCore;
using Sazzle.Domain.Entities;
namespace Sazzle.Infrastructure.Persistence;

public static class DbSeeder
{
    public static async Task SeedSystemRolesAsync(SazzleDbContext context)
    {
        if (await context.Roles.AnyAsync(r => r.OrganizationId == null))
            return;

        var invite = new Permission("members:invite", "Invite new members to the organization");
        var remove = new Permission("members:remove", "Remove members from the organization");

        var manageRoles = new Permission("roles:manage", "Create and assign custom roles");
        var billing = new Permission("billing:write", "Manage billing and subscription");

        var owner = new Role("Owner");
        owner.GrantPermission(invite);
        owner.GrantPermission(remove);
        owner.GrantPermission(manageRoles);
        owner.GrantPermission(billing);

        
        var admin = new Role("Admin");
        admin.GrantPermission(invite);
        admin.GrantPermission(remove);
        
        var member = new Role("Member");
        // no elevate permissions
        
        context.Roles.AddRange(owner, admin, member);
        await context.SaveChangesAsync();
    }
}