
public class CashRegisterDto
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public decimal InitialAmount { get; set; }
    public decimal? FinalAmount { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}