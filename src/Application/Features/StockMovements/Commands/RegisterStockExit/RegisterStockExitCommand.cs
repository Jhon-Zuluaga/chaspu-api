
using MediatR;

public class RegisterStockExitCommand : IRequest<StockMovementDto>
{
    public int ProductId { get; set;}
    public int Quantity { get; set;}
    public string Notes { get; set;} = string.Empty;
}