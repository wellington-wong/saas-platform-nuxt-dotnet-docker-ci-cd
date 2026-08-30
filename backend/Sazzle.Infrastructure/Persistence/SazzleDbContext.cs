using Microsoft.EntityFrameworkCore;
using Sazzle.Domain.Entities;


namespace Sazzle.Infrastructure.Persistence;

public class SazzleDbContext : DbContext
{
    public SazzleDbContext(DbContextOptions<SazzleDbContext> options) : base(options) { }
    public DbSet<User> Users => Set<User>();
    public DbSet<Organization> Organizations => Set<Organization>();
    public DbSet<Team> Teams => Set<Team>();
    public DbSet<OrganizationMember> OrganizationMembers => Set<OrganizationMember>();

    public DbSet<TeamMember> TeamMembers => Set<TeamMember>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();

    public DbSet<Invitation> Invitations => Set<Invitation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SazzleDbContext).Assembly);

		modelBuilder.Entity<Organization>().UsePropertyAccessMode(PropertyAccessMode.Field);
		modelBuilder.Entity<Team>().UsePropertyAccessMode(PropertyAccessMode.Field);
		modelBuilder.Entity<User>().UsePropertyAccessMode(PropertyAccessMode.Field);
		modelBuilder.Entity<Role>().UsePropertyAccessMode(PropertyAccessMode.Field);     
		base.OnModelCreating(modelBuilder);
    }

}