
public class StockMovement : BaseEntity
{
    public int ProductId { get; set; }
    public int UserId { get; set; }
    public StockMovementType Type { get; set;}
    public int Quantity { get; set;}
    public decimal UnitCostPrice { get; set; }
    public decimal? NewSalePrice { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public string? Notes { get; set; }

    // Navigation Properties
    public Product Product { get; set; } = null!;
    public User User { get; set; } = null!;

    // Helper (not mapped)
    public decimal TotalCost => Quantity * UnitCostPrice;
}