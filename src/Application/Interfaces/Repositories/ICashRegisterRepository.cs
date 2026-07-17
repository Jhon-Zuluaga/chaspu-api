
public interface ICashRegisterRepository : IRepository<CashRegister>
{
    Task<CashRegister?> GetByDateAsync(DateOnly date, CancellationToken cancellationToken = default);
    Task<CashRegister?> GetCurrentOpenRegisterAsync(CancellationToken cancellationToken = default);
}