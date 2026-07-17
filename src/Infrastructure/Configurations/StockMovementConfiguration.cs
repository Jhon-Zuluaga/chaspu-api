
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class StockMovementConfiguration : IEntityTypeConfiguration<StockMovement>
{
    public void Configure(EntityTypeBuilder<StockMovement> builder)
    {
        builder.ToTable("StockMovements");

        builder.HasKey(sm => sm.Id);

        builder.Property(sm => sm.Type)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(sm => sm.Quantity)
            .IsRequired();

        builder.Property(sm => sm.UnitCostPrice)
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(sm => sm.NewSalePrice)
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(sm => sm.Notes)
            .HasMaxLength(300);
        
        builder.HasOne(sm => sm.Product)
            .WithMany(p => p.stockMovements)
            .HasForeignKey(sm => sm.Product)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(sm => sm.User)
            .WithMany(u => u.StockMovements)
            .HasForeignKey(sm => sm.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Ignore(sm => sm.TotalCost);
    }
}