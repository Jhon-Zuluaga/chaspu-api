
using AutoMapper;
using MediatR;

public class GetStockMovementsByProductQueryHandler :
    IRequestHandler<GetStockMovementsByProductQuery, List<StockMovementDto>>
{
    private readonly IStockMovementRepository _stockMovementRepository;
    private readonly IMapper _mapper;

    public GetStockMovementsByProductQueryHandler(IStockMovementRepository stockMovementRepository, IMapper mapper)
    {
        _stockMovementRepository = stockMovementRepository;
        _mapper = mapper;
    }

    public async Task<List<StockMovementDto>> Handle(GetStockMovementsByProductQuery request, CancellationToken cancellationToken)
    {
        var movements = await _stockMovementRepository.GetByProductAsync(request.ProductId, cancellationToken);
        return _mapper.Map<List<StockMovementDto>>(movements);
    }
}