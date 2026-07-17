
using AutoMapper;
using MediatR;

public class CheckInCommandHandler : IRequestHandler<CheckInCommand, AttendanceDto>
{
    private readonly IAttendanceRepository _attendanceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public CheckInCommandHandler(
        IAttendanceRepository attendanceRepository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IMapper mapper
    )
    {
        _attendanceRepository = attendanceRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<AttendanceDto> Handle(CheckInCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is null)
            throw new BusinessRuleException("The authenticated user could not be identified.");

        var userId = _currentUserService.UserId.Value;
        var today = DateOnly.FromDateTime(DateTime.Now);

        var existing = await _attendanceRepository.GetOpenAttendanceForUserAsync(userId, today, cancellationToken);

        if (existing is not null)
            throw new BusinessRuleException("There is already an entry marked for today with no recorded exit.");

        var attendance = new Attendance
        {
            UserId = userId,
            Date = today,
            CheckIn = TimeOnly.FromDateTime(DateTime.Now)

        };

        await _attendanceRepository.AddAsync(attendance, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<AttendanceDto>(attendance);

    }
}