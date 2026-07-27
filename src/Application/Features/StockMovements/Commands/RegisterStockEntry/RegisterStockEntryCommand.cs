
using MediatR;

public class RegisterStockEntryCommand : IRequest<StockMovementDto>
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitCostPrice { get; set; }
    public decimal? NewSalePrice { get; set; }
    public string? Notes { get; set; }
}