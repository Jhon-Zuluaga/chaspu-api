
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(p => p.Category)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(p => p.CostPrice)
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(p => p.SalePrice)
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(p => p.CurrentStock)
            .IsRequired();

        builder.Property(p => p.MinStock)
            .IsRequired();

        builder.HasIndex(p => p.Name);

        builder.Ignore(p => p.IsLowStock);
        builder.Ignore(p => p.ProfitMargin);
    }
}