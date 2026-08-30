
using MediatR;

public class GetTodaysSalesTotalQueryHandler : IRequestHandler<GetTodaysSalesTotalQuery, decimal>
{
    private readonly ISaleRepository _saleRepository;

    public GetTodaysSalesTotalQueryHandler(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    public async Task<decimal> Handle(GetTodaysSalesTotalQuery request, CancellationToken cancellationToken)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        return await _saleRepository.GetTotalSalesByDateAsync(today, cancellationToken);
    }
}