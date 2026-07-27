
using AutoMapper;
using MediatR;

public class GetStockMovementsByDateRangeQueryHandler :
    IRequestHandler<GetStockMovementsByDateRangeQuery, List<StockMovementDto>>
{
    private readonly IStockMovementRepository _stockMovementRepository;
    private readonly IMapper _mapper;

    public GetStockMovementsByDateRangeQueryHandler(IStockMovementRepository stockMovementRepository, IMapper mapper)
    {
        _stockMovementRepository = stockMovementRepository;
        _mapper = mapper;
    }

    public async Task<List<StockMovementDto>> Handle(GetStockMovementsByDateRangeQuery request, CancellationToken cancellationToken)
    {
        var movements = await _stockMovementRepository.GetByDateRangeAsync(request.StartDate, request.EndDate, cancellationToken);
        return _mapper.Map<List<StockMovementDto>>(movements);
    }
}