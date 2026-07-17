
using AutoMapper;
using MediatR;

public class GetMyAttendanceHistoryQueryHandler
    : IRequestHandler<GetMyAttendanceHistoryQuery, List<AttendanceDto>>
{
    private readonly IAttendanceRepository _attendanceRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public GetMyAttendanceHistoryQueryHandler(
        IAttendanceRepository attendanceRepository,
        ICurrentUserService currentUserService,
        IMapper mapper
    )
    {
        _attendanceRepository = attendanceRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<List<AttendanceDto>> Handle(GetMyAttendanceHistoryQuery request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is null)
            throw new BusinessRuleException("The authenticated user could not be identified");

        var records = await _attendanceRepository.GetByUserAndDateRangeAsync(
            _currentUserService.UserId.Value, request.StartDate, request.EndDate, cancellationToken
        );

        return _mapper.Map<List<AttendanceDto>>(records);
    }
}