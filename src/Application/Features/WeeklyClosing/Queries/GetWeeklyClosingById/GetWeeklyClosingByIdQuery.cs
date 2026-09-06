
using MediatR;

public class GetWeeklyClosingByIdQuery : IRequest<WeeklyClosingSummaryDto>
{
    public int Id { get; set; }
    public GetWeeklyClosingByIdQuery(int id) => Id = id;
}