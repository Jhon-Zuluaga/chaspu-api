
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CashRegisterConfiguration : IEntityTypeConfiguration<CashRegister>
{
    public void Configure(EntityTypeBuilder<CashRegister> builder)
    {
        builder.ToTable("CashRegisters");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.InitialAmount)
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(c => c.FinalAmount)
            .HasColumnType("decimal(10,2)");

        builder.Property(c => c.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.HasOne(c => c.User)
            .WithMany(u => u.CashRegistersOpened)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(c => c.Date)
            .IsUnique();
    }
}