
using MediatR;

public class CloseCashRegisterCommandHandler : IRequestHandler<CloseCashRegisterCommand, CashRegisterCloseSummaryDto>
{
    private readonly ICashRegisterRepository _cashRegisterRepository;
    private readonly ISaleRepository _saleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CloseCashRegisterCommandHandler(
        ICashRegisterRepository cashRegisterRepository,
        ISaleRepository saleRepository,
        IUnitOfWork unitOfWork
    )
    {
        _cashRegisterRepository = cashRegisterRepository;
        _saleRepository = saleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CashRegisterCloseSummaryDto> Handle(CloseCashRegisterCommand request, CancellationToken cancellationToken)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);

        var cashRegister = await _cashRegisterRepository.GetByDateAsync(today, cancellationToken);

        if (cashRegister is null)
            throw new BusinessRuleException("There is no open box to close today.");

        if (cashRegister.Status == CashRegisterStatus.Closed)
            throw new BusinessRuleException("Today's cash register has already been closed.");

        var startOfDay = today.ToDateTime(TimeOnly.MinValue);
        var enOfDay = today.ToDateTime(TimeOnly.MaxValue);

        var todaysSales = await _saleRepository.GetByDateRangeAsync(startOfDay, enOfDay, cancellationToken);

        var totalCashSales = todaysSales
            .Where(s => s.PaymentMethod == PaymentMethod.Cash)
            .Sum(s => s.TotalAmount);

        var totalTransferSales = todaysSales
            .Where(s => s.PaymentMethod == PaymentMethod.Transfer)
            .Sum(s => s.TotalAmount);

        var expectedCashAmount = cashRegister.InitialAmount + totalCashSales;
        var difference = request.ActualFinalAmount - expectedCashAmount;

        cashRegister.FinalAmount = request.ActualFinalAmount;
        cashRegister.Status = CashRegisterStatus.Closed;

        _cashRegisterRepository.Update(cashRegister);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CashRegisterCloseSummaryDto
        {
            CashRegisterId = cashRegister.Id,
            Date = today,
            InitialAmount = cashRegister.InitialAmount,
            TotalCashSales = totalCashSales,
            TotalTransferSales = totalTransferSales,
            ExpectedCashAmount = expectedCashAmount,
            ActualFinalAmount = request.ActualFinalAmount,
            Difference = difference,
            IsBalanced = difference == 0
        };
    }
}