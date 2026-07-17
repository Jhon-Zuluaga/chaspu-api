
public interface IAttendanceRepository : IRepository<Attendance>
{
    Task<Attendance?> GetOpenAttendanceForUserAsync(int userId, DateOnly date, CancellationToken cancellationToken = default);
    Task<List<Attendance>> GetByUserAndDateRangeAsync(int userId, DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken = default);
    Task<List<Attendance>> GetByDateRangeAsync(DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken = default);
}