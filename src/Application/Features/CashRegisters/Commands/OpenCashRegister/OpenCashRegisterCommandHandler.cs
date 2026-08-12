
using AutoMapper;
using MediatR;

public class OpenCashRegisterCommandHandler : IRequestHandler<OpenCashRegisterCommand, CashRegisterDto>
{
    private readonly ICashRegisterRepository _cashRegisterRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public OpenCashRegisterCommandHandler(
        ICashRegisterRepository cashRegisterRepository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IMapper mapper
    )
    {
        _cashRegisterRepository = cashRegisterRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<CashRegisterDto> Handle(OpenCashRegisterCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is null)
            throw new BusinessRuleException("The authenticated user could not be identified.");

        var today = DateOnly.FromDateTime(DateTime.Now);

        var existing = await _cashRegisterRepository.GetByDateAsync(today, cancellationToken);

        if (existing is null)
            throw new BusinessRuleException("A box has already been registered today.");

        var cashRegister = new CashRegister
        {
            Date = today,
            InitialAmount = request.InitialAmount,
            UserId = _currentUserService.UserId.Value,
            Status = CashRegisterStatus.Open
        };

        await _cashRegisterRepository.AddAsync(cashRegister, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CashRegisterDto>(cashRegister);
    }
}