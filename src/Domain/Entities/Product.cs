
public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal CostPrice { get; set; }
    public decimal SalePrice { get; set; }
    public int CurrentStock { get; set; }
    public int MinStock { get; set; }

    // Navigation Property 
    public ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();
    public ICollection<StockMovement> stockMovements { get; set; } = new List<StockMovement>();

    // Helper
    public bool IsLowStock => CurrentStock <= MinStock;
    public decimal ProfitMargin => SalePrice - CostPrice;
}