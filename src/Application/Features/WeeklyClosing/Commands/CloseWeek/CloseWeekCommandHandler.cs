
using MediatR;

public class CloseWeekCommandHandler : IRequestHandler<CloseWeekCommand, WeeklyClosingSummaryDto>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IUserRepository _userRepository;
    private readonly IAttendanceRepository _attendanceRepository;
    private readonly IProductRepository _productRepository;
    private readonly IWeeklyClosingRepository _weeklyClosingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public CloseWeekCommandHandler(
        ISaleRepository saleRepository,
        IUserRepository userRepository,
        IAttendanceRepository attendanceRepository,
        IProductRepository productRepository,
        IWeeklyClosingRepository weeklyClosingRepository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService
    )
    {
        _saleRepository = saleRepository;
        _userRepository = userRepository;
        _attendanceRepository = attendanceRepository;
        _productRepository = productRepository;
        _weeklyClosingRepository = weeklyClosingRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<WeeklyClosingSummaryDto> Handle(CloseWeekCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is null)
            throw new BusinessRuleException("The authenticated user could not be identified.");

        var startDateTime = request.StartDate.ToDateTime(TimeOnly.MinValue);
        var endDateTime = request.EndDate.ToDateTime(TimeOnly.MaxValue);

        var sales = await _saleRepository.GetByDateRangeAsync(startDateTime, endDateTime, cancellationToken);
        var grossSales = sales.Sum(s => s.TotalAmount);

        var totalCostOfGoodsSold = sales
            .SelectMany(s => s.SaleDetails)
            .Sum(d => d.Quantity * d.Product.CostPrice);

        var allUsers = await _userRepository.GetAllAsync(cancellationToken);
        var workers = allUsers.Where(u => u.Role == UserRole.Worker).ToList();

        var payrollEntities = new List<WeeklyClosingPayroll>();
        var payrollDtos = new List<EmployeePayrollDto>();

        foreach (var worker in workers)
        {
            var attendances = await _attendanceRepository.GetByUserAndDateRangeAsync(
                worker.Id, request.StartDate, request.EndDate, cancellationToken
            );

            var totalWorkedTime = attendances
                .Where(a => a.WorkedHours.HasValue)
                .Aggregate(TimeSpan.Zero, (acc, a) => acc + a.WorkedHours!.Value);

            var salaryPaid = worker.WeeklySalary ?? 0;

            payrollEntities.Add(new WeeklyClosingPayroll
            {
                UserId = worker.Id,
                EmployeeName = worker.Name,
                WeeklySalaryPaid = salaryPaid,
                WorkedHours = totalWorkedTime
            });

            payrollDtos.Add(new EmployeePayrollDto
            {
                UserId = worker.Id,
                Name = worker.Name,
                WorkedHours = $"{(int)totalWorkedTime.TotalHours}h {totalWorkedTime.Minutes}m",
                WeeklySalary = salaryPaid
            });
        }

        var totalPayroll = payrollEntities.Sum(p => p.WeeklySalaryPaid);
        var netProfit = grossSales - totalCostOfGoodsSold - totalPayroll;

        var lowStockProducts = await _productRepository.GetLowStockProductsAsync(cancellationToken);

        var weeklyClosing = new WeeklyClosing
        {
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            GrossSales = grossSales,
            TotalCostOfGoodsSold = totalCostOfGoodsSold,
            NetProfit = netProfit,
            LowStockProductsCount = lowStockProducts.Count,
            ClosedByUserId = _currentUserService.UserId.Value,
            ClosedAt = DateTime.UtcNow,
            Payrolls = payrollEntities
        };

        await _weeklyClosingRepository.AddAsync(weeklyClosing, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new WeeklyClosingSummaryDto
        {
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            GrossSale = grossSales,
            TotalCostOfGoodsSol = totalCostOfGoodsSold,
            TotalPayroll = totalPayroll,
            Employees = payrollDtos,
            LowStockProductsCount = lowStockProducts.Count
        };
    }
}