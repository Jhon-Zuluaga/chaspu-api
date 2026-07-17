
public interface IStockMovementRepository : IRepository<StockMovement>
{
    Task<List<StockMovement>> GetByProductAsync(int productId, CancellationToken cancellationToken = default);
    Task<List<StockMovement>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
}