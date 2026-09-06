
using Microsoft.EntityFrameworkCore;

public class ChaspuDbContext : DbContext
{
    public ChaspuDbContext(DbContextOptions<ChaspuDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Attendance> Attendances => Set<Attendance>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<StockMovement> StockMovements => Set<StockMovement>();
    public DbSet<CashRegister> CashRegisters => Set<CashRegister>();
    public DbSet<Sale> Sales => Set<Sale>();
    public DbSet<SaleDetail> SaleDetails => Set<SaleDetail>();
    public DbSet<WeeklyClosing> WeeklyClosings => Set<WeeklyClosing>();
    public DbSet<WeeklyClosingPayroll> WeeklyClosingPayrolls => Set<WeeklyClosingPayroll>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ChaspuDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}