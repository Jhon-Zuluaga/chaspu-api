

public class StockMovementDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitCostPrice { get; set; }
    public decimal TotalCost { get; set; }
    public decimal? NewSalePrice { get; set; }
    public DateTime Date { get; set; }
    public string? Notes { get; set; }
}