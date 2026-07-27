
using System.Data;
using FluentValidation;

public class RegisterStockEntryCommandValidator : AbstractValidator<RegisterStockEntryCommand>
{
    private readonly IProductRepository _productRepository;

    public RegisterStockEntryCommandValidator(IProductRepository productRepository)
    {
        _productRepository = productRepository;

        RuleFor(x => x.ProductId)
            .MustAsync(async (id, cancellationToken) =>
                await _productRepository.GetByIdAsync(id, cancellationToken) is not null)
            .WithMessage("The product does not exist.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("The amount must be greater than zero.");

        RuleFor(x => x.UnitCostPrice)
            .GreaterThanOrEqualTo(0).WithMessage("The unit cost cannot be negative.");

        RuleFor(X => X.NewSalePrice)
            .GreaterThan(0).When(x => x.NewSalePrice.HasValue)
            .WithMessage("The new selling price must be greater than zero.");

        RuleFor(x => x.Notes)
            .MaximumLength(300);
    }
}