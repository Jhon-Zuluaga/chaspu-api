
using MediatR;

public class CreateSaleCommandHandler : IRequestHandler<CreateSaleCommand, SaleDto>
{
    private readonly IProductRepository _productRepository;
    private readonly ISaleRepository _saleRepository;
    private readonly ICashRegisterRepository _cashRegisterRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public CreateSaleCommandHandler(
        IProductRepository productRepository,
        ISaleRepository saleRepository,
        ICashRegisterRepository cashRegisterRepository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService
    )
    {
        _productRepository = productRepository;
        _saleRepository = saleRepository;
        _cashRegisterRepository = cashRegisterRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<SaleDto> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is null)
            throw new BusinessRuleException("The authenticated user could not be identified.");

        var openRegister = await _cashRegisterRepository.GetCurrentOpenRegisterAsync(cancellationToken);
        if (openRegister is null)
            throw new BusinessRuleException("There is no open box. You must open the box before selling.");

        var saleDetails = new List<SaleDetail>();
        decimal total = 0;
        var responseItems = new List<SaleDetailDto>();

        foreach (var item in request.Items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId, cancellationToken);

            if (product is null)
                throw new NotFoundException(nameof(Product), item.ProductId);

            if (product.CurrentStock < item.Quantity)
                throw new BusinessRuleException($"Insufficient stock for '{product.Name}'. Available: {product.CurrentStock}, Ordered: {item.Quantity}.");

            product.CurrentStock -= item.Quantity;
            _productRepository.Update(product);

            var subtotal = product.SalePrice * item.Quantity;
            total += subtotal;

            saleDetails.Add(new SaleDetail
            {
                ProductId = product.Id,
                Quantity = item.Quantity,
                UnitPrice = product.SalePrice
            });

            responseItems.Add(new SaleDetailDto
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Quantity = item.Quantity,
                UnitPrice = product.SalePrice,
                Subtotal = subtotal
            });
        }

        decimal? changeGiven = null;
        if (request.paymentMethod == PaymentMethod.Cash)
        {
            if (request.AmountReceived!.Value < total)
                throw new BusinessRuleException($"The amount received (${request.AmountReceived.Value}) is less than the total sale amount (${total}).");

            changeGiven = request.AmountReceived.Value - total;
        }

        var sale = new Sale
        {
            Date = DateTime.UtcNow,
            TotalAmount = total,
            PaymentMethod = request.paymentMethod,
            UserId = _currentUserService.UserId.Value,
            SaleDetails = saleDetails
        };

        await _saleRepository.AddAsync(sale, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new SaleDto
        {
            Id = sale.Id,
            Date = sale.Date,
            TotalAmount = sale.TotalAmount,
            PaymentMethod = sale.PaymentMethod.ToString(),
            UserName = _currentUserService.Email ?? string.Empty,
            AmountReceived = request.AmountReceived,
            ChangeGiven = changeGiven,
            Items = responseItems
        };
    }
}