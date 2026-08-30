
using MediatR;

public class CreateSaleCommand : IRequest<SaleDto>
{
    public List<CreateSaleItemDto> Items { get; set; } = new();
    public PaymentMethod paymentMethod { get; set; }
    public decimal? AmountReceived { get; set; }

}