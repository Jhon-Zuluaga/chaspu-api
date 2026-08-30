
using MediatR;

public class GetSaleByIdQuery : IRequest<SaleDto>
{
    public int Id { get; set; }
    public GetSaleByIdQuery(int id) => Id = id;
}