
using MediatR;

public class OpenCashRegisterCommand : IRequest<CashRegisterDto>
{
    public decimal InitialAmount { get; set; }
}