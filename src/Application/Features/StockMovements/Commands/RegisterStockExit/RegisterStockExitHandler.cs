
using AutoMapper;
using MediatR;

public class RegisterStockExitHandler : IRequestHandler<RegisterStockExitCommand, StockMovementDto>
{
    private readonly IProductRepository _productRepository;
    private readonly IStockMovementRepository _stockMovementRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public RegisterStockExitHandler(
        IProductRepository productRepository,
        IStockMovementRepository stockMovementRepository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IMapper mapper
    )
    {
        _productRepository = productRepository;
        _stockMovementRepository = stockMovementRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<StockMovementDto> Handle(RegisterStockExitCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is null)
            throw new BusinessRuleException("The user authenticated user could not be identified.");

        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);

        if (product is null)
            throw new NotFoundException(nameof(Product), request.ProductId);

        if (product.CurrentStock < request.Quantity)
            throw new BusinessRuleException(
                $"There isn't enough. Stock available: {product.CurrentStock}, Request: {request.Quantity}."
            );

        product.CurrentStock -= request.Quantity;
        _productRepository.Update(product);

        var movement = new StockMovement
        {
            ProductId = product.Id,
            UserId = _currentUserService.UserId.Value,
            Type = StockMovementType.Adjusment,
            Quantity = request.Quantity,
            UnitCostPrice = product.CostPrice,
            Notes = request.Notes,
            Date = DateTime.UtcNow
        };

        await _stockMovementRepository.AddAsync(movement, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        movement.Product = product;
        return _mapper.Map<StockMovementDto>(movement);
    }
}