
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class SaleDetailConfiguration : IEntityTypeConfiguration<SaleDetail>
{
    public void Configure(EntityTypeBuilder<SaleDetail> builder)
    {
        builder.ToTable("SaleDetails");

        builder.HasKey(sd => sd.Id);

        builder.Property(sd => sd.Quantity)
            .IsRequired();

        builder.Property(sd => sd.UnitPrice)
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.HasOne(sd => sd.Sale)
            .WithMany(s => s.SaleDetails)
            .HasForeignKey(sd => sd.SaleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(sd => sd.Product)
            .WithMany(p => p.SaleDetails)
            .HasForeignKey(sd => sd.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Ignore(sd => sd.Subtotal);
    }
}