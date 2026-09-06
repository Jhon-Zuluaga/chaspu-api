
using MediatR;

public class GetWeeklyClosingByIdQueryHandler : IRequestHandler<GetWeeklyClosingByIdQuery, WeeklyClosingSummaryDto>
{
    private readonly IWeeklyClosingRepository _weeklyClosingRepository;

    public GetWeeklyClosingByIdQueryHandler(IWeeklyClosingRepository weeklyClosingRepository)
    {
        _weeklyClosingRepository = weeklyClosingRepository;
    }

    public async Task<WeeklyClosingSummaryDto> Handle(GetWeeklyClosingByIdQuery request, CancellationToken cancellationToken)
    {
        var closing = await _weeklyClosingRepository.GetByIdWithPayrollsAsync(request.Id, cancellationToken);

        if (closing is null)
            throw new NotFoundException(nameof(WeeklyClosing), request.Id);

        return new WeeklyClosingSummaryDto
        {
            StartDate = closing.StartDate,
            EndDate = closing.EndDate,
            GrossSale = closing.GrossSales,
            TotalCostOfGoodsSol = closing.TotalCostOfGoodsSold,
            TotalPayroll = closing.TotalPayroll,
            NetProfit = closing.NetProfit,
            LowStockProductsCount = closing.LowStockProductsCount,
            Employees = closing.Payrolls.Select(p => new EmployeePayrollDto
            {
                UserId = p.UserId,
                Name = p.EmployeeName,
                WeeklySalary = p.WeeklySalaryPaid,
                WorkedHours = $"{(int)p.WorkedHours.TotalHours}h {p.WorkedHours.Minutes}m"
            }).ToList()
        };
    }
}