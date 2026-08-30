
using MediatR;

public class GetSalesByDateRangeQuery : IRequest<List<SaleDto>>
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}