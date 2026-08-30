
using FluentValidation;

public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
{
    public CreateSaleCommandValidator()
    {
        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("The shopping cart cannot be empty.");

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.ProductId)
                .GreaterThan(0).WithMessage("Invalid item in the shopping cart.");

            item.RuleFor(i => i.Quantity)
                .GreaterThan(0).WithMessage("The amount must be greater than zero.");

        });

        RuleFor(x => x.paymentMethod)
            .IsInEnum().WithMessage("Invalid payment method.");

        RuleFor(x => x.AmountReceived)
            .NotNull().WithMessage("You must indicate the amount received in cash.")
            .When(x => x.paymentMethod == PaymentMethod.Cash);
    }
}