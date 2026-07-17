
public interface ISaleRepository : IRepository<Sale>
{
    Task<List<Sale>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    Task<decimal> GetTotalSalesByDateAsync(DateOnly date, CancellationToken cancellationToken = default);
    Task<Sale?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default);
}