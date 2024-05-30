using CRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRM.Persistence.Configurations.CRM;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");
        builder.Property(x => x.Name).HasMaxLength(50);
        builder.Property(x => x.Description).HasMaxLength(256);
        builder.Property(x => x.ProductStock);
        builder.Property(x => x.Price).HasColumnType("decimal(10,2)");
    }
}