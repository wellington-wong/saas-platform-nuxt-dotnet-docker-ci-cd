using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sazzle.Domain.Entities;

namespace Sazzle.Infrastructure.Persistence.Configurations;

public class OrganizationMemberConfiguration : IEntityTypeConfiguration<OrganizationMember>
{
    public void Configure(EntityTypeBuilder<OrganizationMember> builder)
    {
        builder.ToTable("organization_members");

        builder.HasKey(m => m.Id);
        
        
        // a user can only be a member of a given org once
        builder.HasIndex(m => new { m.UserId, m.OrganizationId })
            .IsUnique();

        builder.HasOne<Role>()
            .WithMany()
            .HasForeignKey(m => m.RoleId)
            .OnDelete(DeleteBehavior.Restrict); // don't cascade-delete membership if a Role is deleted
    }
}