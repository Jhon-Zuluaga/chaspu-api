
using MediatR;

public class GetStockMovementsByProductQuery : IRequest<List<StockMovementDto>>
{
    public int ProductId { get; set; }
    public GetStockMovementsByProductQuery(int productId) => ProductId = productId;
}