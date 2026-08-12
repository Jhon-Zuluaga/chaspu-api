
using FluentValidation;

public class CloseCashRegisterCommandValidator : AbstractValidator<CloseCashRegisterCommand>
{
    public CloseCashRegisterCommandValidator()
    {
        RuleFor(x => x.ActualFinalAmount)
            .GreaterThanOrEqualTo(0).WithMessage("The final amount cannot be negative");
    }
}