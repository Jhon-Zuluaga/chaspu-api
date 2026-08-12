
using AutoMapper;
using MediatR;

public class GetCashRegisterByDateQueryHandler : IRequestHandler<GetCashRegisterByDateQuery, CashRegisterDto>
{
    private readonly ICashRegisterRepository _cashRegisterRepository;
    private readonly IMapper _mapper;

    public GetCashRegisterByDateQueryHandler(ICashRegisterRepository cashRegisterRepository, IMapper mapper)
    {
        _cashRegisterRepository = cashRegisterRepository;
        _mapper = mapper;
    }

    public async Task<CashRegisterDto> Handle(GetCashRegisterByDateQuery request, CancellationToken cancellationToken)
    {
        var cashRegister = await _cashRegisterRepository.GetByDateAsync(request.Date, cancellationToken);

        if (cashRegister is null)
            throw new NotFoundException(nameof(CashRegister), request.Date);

        return _mapper.Map<CashRegisterDto>(cashRegister);
    }
}