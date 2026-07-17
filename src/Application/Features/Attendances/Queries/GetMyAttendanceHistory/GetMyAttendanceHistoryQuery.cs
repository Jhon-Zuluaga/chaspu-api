
using MediatR;

public class GetMyAttendanceHistoryQuery : IRequest<List<AttendanceDto>>
{
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
}