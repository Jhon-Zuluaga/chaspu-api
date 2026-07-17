
using AutoMapper;
using MediatR;

public class CheckOutCommandHandler : IRequestHandler<CheckOutCommand, AttendanceDto>
{
    private readonly IAttendanceRepository _attendanceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public CheckOutCommandHandler(
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

    public async Task<AttendanceDto> Handle(CheckOutCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is null)
            throw new BusinessRuleException("The authenticated user could not be identified.");

        var userId = _currentUserService.UserId.Value;
        var today = DateOnly.FromDateTime(DateTime.Now);

        var attendance = await _attendanceRepository.GetOpenAttendanceForUserAsync(userId, today, cancellationToken);

        if (attendance is null)
            throw new BusinessRuleException("There is no entry marked for today to record the departure");

        attendance.CheckOut = TimeOnly.FromDateTime(DateTime.Now);

        _attendanceRepository.Update(attendance);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<AttendanceDto>(attendance);
    }
}