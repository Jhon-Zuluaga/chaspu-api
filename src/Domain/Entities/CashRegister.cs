
public class CashRegister : BaseEntity
{
    public DateOnly Date { get; set; }
    public decimal InitialAmount { get; set; }
    public decimal? FinalAmount { get; set; }
    public int UserId { get; set; }
    public CashRegisterStatus Status { get; set; } = CashRegisterStatus.Open;

    // Navigation Property
    public User User { get; set; } = null!;
}