
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class WeeklyClosingPayrollConfiguration : IEntityTypeConfiguration<WeeklyClosingPayroll>
{
    public void Configure(EntityTypeBuilder<WeeklyClosingPayroll> builder)
    {
        builder.ToTable("WeeklyClosingPayrolls");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.EmployeeName).IsRequired().HasMaxLength(100);
        builder.Property(p => p.WeeklySalaryPaid).HasColumnType("decimal(10,2)");

        builder.HasOne(p => p.WeeklyClosing)
            .WithMany(w => w.Payrolls)
            .HasForeignKey(p => p.WeeklyClosingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.User)
            .WithMany(u => u.weeklyClosingPayrolls)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}