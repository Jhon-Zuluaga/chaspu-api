
using AutoMapper;
using MediatR;

public class RegisterStockEntryCommandHandler : IRequestHandler<RegisterStockEntryCommand, StockMovementDto>
{
    private readonly IProductRepository _productRepository;
    private readonly IStockMovementRepository _stockMovementRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public RegisterStockEntryCommandHandler(
        IProductRepository productRepository,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork,
        IStockMovementRepository stockMovementRepository,
        IMapper mapper
    )
    {
        _productRepository = productRepository;
        _stockMovementRepository = stockMovementRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<StockMovementDto> Handle(RegisterStockEntryCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is null)
            throw new BusinessRuleException("The authenticated user could not be identified.");

        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);

        if (product is null)
            throw new NotFoundException(nameof(Product), request.ProductId);

        // Update product inventory
        product.CurrentStock += request.Quantity;

        // Update the cost to the most recent one
        product.CostPrice = request.UnitCostPrice;

        // If a new selling price is available, it updates it
        if (request.NewSalePrice.HasValue)
        {
            product.SalePrice = request.NewSalePrice.Value;
        }

        _productRepository.Update(product);

        // Records the movement for traceability
        var movement = new StockMovement
        {
            ProductId = product.Id,
            UserId = _currentUserService.UserId.Value,
            Type = StockMovementType.Restock,
            Quantity = request.Quantity,
            UnitCostPrice = request.UnitCostPrice,
            NewSalePrice = request.NewSalePrice,
            Notes = request.Notes,
            Date = DateTime.UtcNow
        };

        await _stockMovementRepository.AddAsync(movement, cancellationToken);

        // All in a single transaction; if anything goes wrong, nothing is saved.
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        movement.Product = product;
        return _mapper.Map<StockMovementDto>(movement);
    }
}