using CRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRM.Infrastructure.Persistence.Configurations.Crm;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");
        builder.Property(x => x.Name).HasMaxLength(50);
        builder.Property(x => x.Description).HasMaxLength(256);
        builder.Property(x => x.ProductStock);
        builder.Property(x => x.Price).HasColumnType("decimal(10,2)");
        builder.Property(x => x.CreatedBy);
        
        builder.HasIndex(x => x.Name).IsUnique().HasDatabaseName("Product_Name");
    }
}