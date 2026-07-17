
using MediatR;

public class GetAttendanceByDateRangeQuery : IRequest<List<AttendanceDto>>
{
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
}