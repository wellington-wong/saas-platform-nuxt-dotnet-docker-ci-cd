using Microsoft.EntityFrameworkCore;
using Sazzle.Domain.Entities;
namespace Sazzle.Infrastructure.Persistence;

public static class DbSeeder
{
    public static async Task SeedSystemRolesAsync(SazzleDbContext context)
    {
        if (await context.Roles.AnyAsync(r => r.OrganizationId == null))
            return; // already seeded

        var owner = new Role("Owner");
        var admin = new Role("Admin");
        var member = new Role("Member");

        context.Roles.AddRange(owner, admin, member);
        await context.SaveChangesAsync();
    }
}