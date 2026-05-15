using AutoMapper;
using CatalogoPOP.Application.DTOs;
using CatalogoPOP.Application.Queries;
using CatalogoPOP.Domain.Interfaces;
using MediatR;

namespace CatalogoPOP.Application.Handlers;

public class ObterProcedimentoPorIdQueryHandler : IRequestHandler<ObterProcedimentoPorIdQuery, ProcedimentoDTO?>
{
    private readonly IProcedimentoRepository _repository;
    private readonly IMapper _mapper;

    public ObterProcedimentoPorIdQueryHandler(IProcedimentoRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ProcedimentoDTO?> Handle(ObterProcedimentoPorIdQuery request, CancellationToken cancellationToken)
    {
        var pop = await _repository.ObterPorIdAsync(request.Id);
        return _mapper.Map<ProcedimentoDTO>(pop);
    }
}
