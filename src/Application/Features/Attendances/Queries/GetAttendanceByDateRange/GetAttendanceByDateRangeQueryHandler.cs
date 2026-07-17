
using AutoMapper;
using MediatR;

public class GetAttendanceByDateRangeQueryHandler
    : IRequestHandler<GetAttendanceByDateRangeQuery, List<AttendanceDto>>
{
    private readonly IAttendanceRepository _attendanceRepository;
    private readonly IMapper _mapper;

    public GetAttendanceByDateRangeQueryHandler(IAttendanceRepository attendanceRepository, IMapper mapper)
    {
        _attendanceRepository = attendanceRepository;
        _mapper = mapper;
    }

    public async Task<List<AttendanceDto>> Handle(GetAttendanceByDateRangeQuery request, CancellationToken cancellationToken)
    {
        var records = await _attendanceRepository.GetByDateRangeAsync(request.StartDate, request.EndDate, cancellationToken);
        return _mapper.Map<List<AttendanceDto>>(records);
    }
}