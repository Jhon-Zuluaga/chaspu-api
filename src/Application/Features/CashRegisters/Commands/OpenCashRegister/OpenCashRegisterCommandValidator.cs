
using FluentValidation;

public class OpenCashRegisterCommandValidator : AbstractValidator<OpenCashRegisterCommand>
{
    public OpenCashRegisterCommandValidator()
    {
        RuleFor(x => x.InitialAmount)
            .GreaterThanOrEqualTo(0).WithMessage("The initial amount cannot be negative.");
    }
}