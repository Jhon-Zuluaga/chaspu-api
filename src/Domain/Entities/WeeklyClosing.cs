
public class WeeklyClosing : BaseEntity
{
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }

    public decimal GrossSales { get; set; }
    public decimal TotalCostOfGoodsSold { get; set; }
    public decimal TotalPayroll { get; set; }
    public decimal NetProfit { get; set; }
    public int LowStockProductsCount { get; set; }

    public int ClosedByUserId { get; set; }
    public DateTime ClosedAt { get; set; } = DateTime.UtcNow;

    public User ClosedByUser { get; set; } = null!;
    public ICollection<WeeklyClosingPayroll> Payrolls { get; set; } = new List<WeeklyClosingPayroll>();
}