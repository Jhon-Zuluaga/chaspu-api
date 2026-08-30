
using AutoMapper;
using MediatR;

public class UpdateUserSalaryCommandHandler : IRequestHandler <UpdateUserSalaryCommand, UserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateUserSalaryCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(UpdateUserSalaryCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if(user is null)
            throw new NotFoundException(nameof(User), request.UserId);
        
        user.WeeklySalary = request.WeeklySalary;

        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<UserDto>(user);
    }
}