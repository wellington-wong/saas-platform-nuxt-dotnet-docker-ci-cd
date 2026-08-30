using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sazzle.Domain.Entities;

namespace Sazzle.Infrastructure.Persistence.Configurations;

public class InvitationConfiguration : IEntityTypeConfiguration<Invitation>
{
    public void Configure(EntityTypeBuilder<Invitation> builder)
    {
        builder.ToTable("invitations");
        builder.HasKey(i => i.Id);


        builder.Property(i => i.Email).IsRequired().HasMaxLength(256);
        builder.Property(i => i.Token).IsRequired();
        builder.HasIndex(i => i.Token).IsUnique();
        builder.Property(i => i.Status).HasConversion<string>().HasMaxLength(20);
    }
}