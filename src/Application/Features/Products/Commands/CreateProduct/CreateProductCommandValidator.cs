
using FluentValidation;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    private readonly IProductRepository _productRepository;

    public CreateProductCommandValidator(IProductRepository productRepository)
    {
        _productRepository = productRepository;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("The producto name is required.")
            .MaximumLength(150)
            .MustAsync(async (name, cancellationToken) =>
                await _productRepository.GetByNameAsync(name, cancellationToken) is null)
            .WithMessage("There is already a product with that name");

        RuleFor(x => x.Category)
            .NotEmpty().WithMessage("The category is required.")
            .MaximumLength(80);

        RuleFor(x => x.CostPrice)
            .GreaterThanOrEqualTo(0).WithMessage("The cost price cannot be negative.");

        RuleFor(x => x.SalePrice)
            .GreaterThan(0).WithMessage("The selling price must be greater than zero.")
            .GreaterThanOrEqualTo(x => x.CostPrice)
            .WithMessage("The selling price cannot be lower than the cost price.");

        RuleFor(x => x.CurrentStock)
            .GreaterThanOrEqualTo(0).WithMessage("Stock cannot be negative.");

        RuleFor(x => x.MinStock)
            .GreaterThanOrEqualTo(0).WithMessage("The minium stock level cannot be negative.");
    }
}