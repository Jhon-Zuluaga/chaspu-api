
using MediatR;

public class GetSaleByIdQueryHandler : IRequestHandler<GetSaleByIdQuery, SaleDto>
{
    private readonly ISaleRepository _saleRepository;

    public GetSaleByIdQueryHandler(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    public async Task<SaleDto> Handle(GetSaleByIdQuery request, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetByIdWithDetailsAsync(request.Id, cancellationToken);

        if (sale is null)
            throw new NotFoundException(nameof(Sale), request.Id);

        return new SaleDto
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
        };
    }
}