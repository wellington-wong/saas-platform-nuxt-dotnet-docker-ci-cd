using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sazzle.Domain.Entities;


namespace Sazzle.Infrastructure.Persistence.Configurations;

public class TeamMemberConfiguration : IEntityTypeConfiguration<TeamMember>
{
    public void Configure(EntityTypeBuilder<TeamMember> builder)
    {
        builder.ToTable("team_members");
        builder.HasKey(m => m.Id);

        builder.HasIndex(m => new { m.UserId, m.TeamId })
            .IsUnique();
    }
}