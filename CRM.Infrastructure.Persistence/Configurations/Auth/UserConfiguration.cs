using CRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRM.Infrastructure.Persistence.Configurations.Auth;

public class UserConfiguration: IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.Property(x => x.Email).HasMaxLength(50);
        builder.Property(x => x.Name).HasMaxLength(100);
        builder.Property(x => x.PasswordHash).HasMaxLength(256);
        builder.Property(x => x.RefreshToken).HasMaxLength(256);
        builder.Property(x => x.RefreshTokenExpires).HasColumnType("datetime");
        
        builder.HasIndex(x => x.Email).IsUnique().HasDatabaseName("User_Email");

        builder.Property(x => x.Email)
            .HasConversion(e => e.ToLower(), e => e);
    }
}