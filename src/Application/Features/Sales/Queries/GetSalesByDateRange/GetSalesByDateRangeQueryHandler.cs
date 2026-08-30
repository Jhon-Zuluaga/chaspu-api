
using MediatR;

public class GetSalesByDateRangeQueryHandler : IRequestHandler<GetSalesByDateRangeQuery, List<SaleDto>>
{
    private readonly ISaleRepository _saleRepository;

    public GetSalesByDateRangeQueryHandler(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    public async Task<List<SaleDto>> Handle(GetSalesByDateRangeQuery request, CancellationToken cancellationToken)
    {
        var sales = await _saleRepository.GetByDateRangeAsync(request.StartDate, request.EndDate, cancellationToken);

        return sales.Select(sale => new SaleDto
        {
            Id = sale.Id,
            Date = sale.Date,
            TotalAmount = sale.TotalAmount,
            PaymentMethod = sale.PaymentMethod.ToString(),
            UserName = sale.User.Name,
            Items = sale.SaleDetails.Select(d => new SaleDetailDto
            {
                ProductId = d.ProductId,
                ProductName = d.Product.Name,
                Quantity = d.Quantity,
                UnitPrice = d.UnitPrice,
                Subtotal = d.Subtotal
            }).ToList()
        }).ToList();
    }
}