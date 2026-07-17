
public class User : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; }

    // Navigation Properties
    public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    public ICollection<CashRegister> CashRegistersOpened { get; set; } = new List<CashRegister>();
    public ICollection<Sale> Sales { get; set; } = new List<Sale>();
    public ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
}