
using MediatR;

public class GetWeeklyClosingHistoryQueryHandler
    : IRequestHandler<GetWeeklyClosingHistoryQuery, List<WeeklyClosingSummaryDto>>
{
    private readonly IWeeklyClosingRepository _weeklyClosingRepository;

    public GetWeeklyClosingHistoryQueryHandler(IWeeklyClosingRepository weeklyClosingRepository)
    {
        _weeklyClosingRepository = weeklyClosingRepository;
    }

    public async Task<List<WeeklyClosingSummaryDto>> Handle(
        GetWeeklyClosingHistoryQuery request, CancellationToken cancellationToken)
    {
        var closings = await _weeklyClosingRepository.GetAllOrderedByDateDescAsync(cancellationToken);

        return closings.Select(c => new WeeklyClosingSummaryDto
        {
            StartDate = c.StartDate,
            EndDate = c.EndDate,
            GrossSale = c.GrossSales,
            TotalCostOfGoodsSol = c.TotalCostOfGoodsSold,
            TotalPayroll = c.TotalPayroll,
            NetProfit = c.NetProfit,
            LowStockProductsCount = c.LowStockProductsCount,
            Employees = c.Payrolls.Select(p => new EmployeePayrollDto
            {
                UserId = p.UserId,
                Name = p.EmployeeName,
                WeeklySalary = p.WeeklySalaryPaid,
                WorkedHours = $"{(int)p.WorkedHours.TotalHours}h {p.WorkedHours.Minutes}m"
            }).ToList()
        }).ToList();
    }


}