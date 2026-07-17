
public interface IProductRepository : IRepository<Product>
{
    Task<Product?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<List<Product>> GetByCategoryAsync(string category, CancellationToken cancellationToken = default);
    Task<List<Product>> GetLowStockProductsAsync(CancellationToken cancellationToken = default);
    Task<List<Product>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}