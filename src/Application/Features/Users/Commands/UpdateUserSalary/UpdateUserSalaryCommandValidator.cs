
using FluentValidation;

public class UpdateUserSalaryCommandValidator : AbstractValidator<UpdateUserSalaryCommand>
{
    public UpdateUserSalaryCommandValidator()
    {
        RuleFor(x => x.WeeklySalary)
            .GreaterThan(0).WithMessage("The weekly salary must be greater than zero.");
    }
}