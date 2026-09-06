
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class WeeklyClosingConfiguration : IEntityTypeConfiguration<WeeklyClosing>
{
    public void Configure(EntityTypeBuilder<WeeklyClosing> builder)
    {
        builder.ToTable("WeeklyClosings");

        builder.HasKey(w => w.Id);

        builder.Property(w => w.GrossSales).HasColumnType("decimal(10,2)");
        builder.Property(w => w.TotalCostOfGoodsSold).HasColumnType("decimal(10,2)");
        builder.Property(w => w.TotalPayroll).HasColumnType("decimal(10,2)");
        builder.Property(w => w.NetProfit).HasColumnType("decimal(10,2)");

        builder.HasOne(W => W.ClosedByUser)
            .WithMany(u => u.WeeklyClosingsMade)
            .HasForeignKey(w => w.ClosedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(w => new { w.StartDate, w.EndDate })
            .IsUnique();
    }
}