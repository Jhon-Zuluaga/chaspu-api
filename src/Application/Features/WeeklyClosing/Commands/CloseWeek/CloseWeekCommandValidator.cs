
using System.Data;
using FluentValidation;

public class CloseWeekCommandValidator : AbstractValidator<CloseWeekCommand>
{
    private readonly IWeeklyClosingRepository _weeklyClosingRepository;

    public CloseWeekCommandValidator(IWeeklyClosingRepository weeklyClosingRepository)
    {
        _weeklyClosingRepository = weeklyClosingRepository;

        RuleFor(x => x.EndDate)
            .GreaterThanOrEqualTo(X => X.StartDate)
            .WithMessage("The end date cannot be earlier than the start date.");
        
        RuleFor(x => x)
            .MustAsync(async (cmd, cancellationToken) =>
                await _weeklyClosingRepository.GetByDateRangeAsync(cmd.StartDate, cmd.EndDate, cancellationToken) is null)
            .WithMessage("A closure has already been recorded for this date range.");
    }
}