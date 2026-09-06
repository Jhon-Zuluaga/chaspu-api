
using MediatR;

namespace Application.Features.WeeklyClosing.Queries.GetWeeklyClosingSummary;

public class GetWeeklyClosingSummaryQueryHandler
    : IRequestHandler<GetWeeklyClosingSummaryQuery, WeeklyClosingSummaryDto>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IUserRepository _userRepository;
    private readonly IAttendanceRepository _attendanceRepository;
    private readonly IProductRepository _productRepository;

    public GetWeeklyClosingSummaryQueryHandler(
        ISaleRepository saleRepository,
        IUserRepository userRepository,
        IAttendanceRepository attendanceRepository,
        IProductRepository productRepository)
    {
        _saleRepository = saleRepository;
        _userRepository = userRepository;
        _attendanceRepository = attendanceRepository;
        _productRepository = productRepository;
    }

    public async Task<WeeklyClosingSummaryDto> Handle(
        GetWeeklyClosingSummaryQuery request, CancellationToken cancellationToken)
    {
        var startDateTime = request.StartDate.ToDateTime(TimeOnly.MinValue);
        var endDateTime = request.EndDate.ToDateTime(TimeOnly.MaxValue);

        // 1. Ventas de la semana (con detalles y productos incluidos)
        var sales = await _saleRepository.GetByDateRangeAsync(startDateTime, endDateTime, cancellationToken);

        var grossSales = sales.Sum(s => s.TotalAmount);

        // 2. Costo de la mercancía vendida: Quantity * Product.CostPrice actual, por cada línea de cada venta
        var totalCostOfGoodsSold = sales
            .SelectMany(s => s.SaleDetails)
            .Sum(d => d.Quantity * d.Product.CostPrice);

        // 3. Nómina: todos los trabajadores (no admins) con su sueldo semanal vigente
        var allUsers = await _userRepository.GetAllAsync(cancellationToken);
        var workers = allUsers.Where(u => u.Role == UserRole.Worker).ToList();

        var employeePayrolls = new List<EmployeePayrollDto>();

        foreach (var worker in workers)
        {
            var attendances = await _attendanceRepository.GetByUserAndDateRangeAsync(
                worker.Id, request.StartDate, request.EndDate, cancellationToken);

            var totalWorkedTime = attendances
                .Where(a => a.WorkedHours.HasValue)
                .Aggregate(TimeSpan.Zero, (acc, a) => acc + a.WorkedHours!.Value);

            employeePayrolls.Add(new EmployeePayrollDto
            {
                UserId = worker.Id,
                Name = worker.Name,
                WorkedHours = $"{(int)totalWorkedTime.TotalHours}h {totalWorkedTime.Minutes}m",
                WeeklySalary = worker.WeeklySalary ?? 0
            });
        }

        var totalPayroll = employeePayrolls.Sum(e => e.WeeklySalary);

        // 4. Utilidad neta
        var netProfit = grossSales - totalCostOfGoodsSold - totalPayroll;

        // 5. Stock crítico para la alerta del reporte (Bloque 8)
        var lowStockProducts = await _productRepository.GetLowStockProductsAsync(cancellationToken);

        return new WeeklyClosingSummaryDto
        {
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            GrossSale = grossSales,
            TotalCostOfGoodsSol = totalCostOfGoodsSold,
            TotalPayroll = totalPayroll,
            NetProfit = netProfit,
            Employees = employeePayrolls,
            LowStockProductsCount = lowStockProducts.Count
        };
    }
}