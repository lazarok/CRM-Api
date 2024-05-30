using CRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRM.Infrastructure.Persistence.Configurations.Auth;

public class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
{
    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder.ToTable("Organizations");
        builder.Property(x => x.Name).HasMaxLength(100);
        builder.Property(x => x.SlugTenant).HasMaxLength(256);
        
        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasMany(s => s.Users)
            .WithOne(p => p.Organization)
            .HasForeignKey(s => s.OrganizationId)
            .OnDelete(DeleteBehavior.ClientNoAction);
    }
}