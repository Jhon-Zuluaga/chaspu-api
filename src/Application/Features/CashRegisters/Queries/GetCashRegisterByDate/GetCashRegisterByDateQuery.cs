
using MediatR;

public class GetCashRegisterByDateQuery : IRequest<CashRegisterDto>
{
    public DateOnly Date { get; set; }
    public GetCashRegisterByDateQuery(DateOnly date) => Date = date;
}