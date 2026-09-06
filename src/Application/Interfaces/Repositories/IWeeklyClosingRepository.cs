
public interface IWeeklyClosingRepository : IRepository<WeeklyClosing>
{
    Task<WeeklyClosing?> GetByDateRangeAsync(DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken = default);
    Task<List<WeeklyClosing>> GetAllOrderedByDateDescAsync(CancellationToken cancellationToken = default);
    Task<WeeklyClosing?> GetByIdWithPayrollsAsync(int id, CancellationToken cancellationToken = default);
}