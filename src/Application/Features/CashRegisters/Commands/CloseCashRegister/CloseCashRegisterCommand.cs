
using MediatR;

public class CloseCashRegisterCommand : IRequest<CashRegisterCloseSummaryDto>
{
    public decimal ActualFinalAmount { get; set; }
}