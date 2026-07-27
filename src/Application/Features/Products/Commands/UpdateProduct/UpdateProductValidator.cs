using System.Data;
using FluentValidation;

public class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Category).NotEmpty().MaximumLength(150);
        RuleFor(x => x.CostPrice).GreaterThanOrEqualTo(0);
        RuleFor(x => x.SalePrice)
            .GreaterThan(0)
            .GreaterThanOrEqualTo(x => x.CostPrice)
            .WithMessage("The selling price cannot be lower than the cost price.");
        RuleFor(x => x.MinStock).GreaterThanOrEqualTo(0);
    }
}