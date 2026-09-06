
using MediatR;

public class GetWeeklyClosingSummaryQuery : IRequest<WeeklyClosingSummaryDto>
{
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
}