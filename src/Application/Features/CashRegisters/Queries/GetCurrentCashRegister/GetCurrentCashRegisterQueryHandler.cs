
using AutoMapper;
using MediatR;

public class GetCurrentCashRegisterQueryHandler : IRequestHandler<GetCurrentCashRegisterQuery, CashRegisterDto?>
{
    private readonly ICashRegisterRepository _cashRegisterRepository;
    private readonly IMapper _mapper;

    public GetCurrentCashRegisterQueryHandler(ICashRegisterRepository cashRegisterRepository, IMapper mapper)
    {
        _cashRegisterRepository = cashRegisterRepository;
        _mapper = mapper;
    }

    public async Task<CashRegisterDto?> Handle(GetCurrentCashRegisterQuery request, CancellationToken cancellationToken)
    {
        var cashRegister = await _cashRegisterRepository.GetCurrentOpenRegisterAsync(cancellationToken);
        return cashRegister is null ? null : _mapper.Map<CashRegisterDto>(cashRegister);
    }
}