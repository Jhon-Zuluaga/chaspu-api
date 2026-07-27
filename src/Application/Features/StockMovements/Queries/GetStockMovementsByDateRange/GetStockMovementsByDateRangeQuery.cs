
using MediatR;

public class GetStockMovementsByDateRangeQuery : IRequest<List<StockMovementDto>>
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}