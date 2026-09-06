
using MediatR;

public class CloseWeekCommand : IRequest<WeeklyClosingSummaryDto>
{
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
}