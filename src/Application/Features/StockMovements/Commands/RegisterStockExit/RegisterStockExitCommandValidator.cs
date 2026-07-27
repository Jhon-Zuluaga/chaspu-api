
using FluentValidation;

public class RegisterStockExitCommandValidator : AbstractValidator<RegisterStockExitCommand>
{
    private readonly IProductRepository _productRepository;

    public RegisterStockExitCommandValidator(IProductRepository productRepository)
    {
        _productRepository = productRepository;

        RuleFor(x => x.ProductId)
            .MustAsync(async (id, cancellationToken) =>
                await _productRepository.GetByIdAsync(id, cancellationToken) is not null)
            .WithMessage("The product does not exist.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("The amount must be greater than zero.");

        RuleFor(x => x.Notes)
            .NotEmpty().WithMessage("You must indicate the reason for the outbound shipment. (Example; damaged product, inventory adjustment)")
            .MaximumLength(300);
    }
}