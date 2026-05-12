using AutoMapper;
using CatalogoPOP.Application.DTOs;
using CatalogoPOP.Application.Queries;
using CatalogoPOP.Domain.Interfaces;
using MediatR;

namespace CatalogoPOP.Application.Handlers;

/// <summary>
/// Manipulador da consulta de todos os procedimentos.
/// Busca no repositório e devolve uma lista de DTOs.
/// </summary>
public class ObterTodosProcedimentosQueryHandler : IRequestHandler<ObterTodosProcedimentosQuery, IEnumerable<ProcedimentoDTO>>
{
    private readonly IProcedimentoRepository _repository;
    private readonly IMapper _mapper;

    public ObterTodosProcedimentosQueryHandler(IProcedimentoRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProcedimentoDTO>> Handle(ObterTodosProcedimentosQuery request, CancellationToken cancellationToken)
    {
        // 1. Busca os dados no banco através do repositório
        var procedimentos = await _repository.ObterTodosAsync();

        // 2. Converte a lista de Entidades em uma lista de DTOs usando AutoMapper
        return _mapper.Map<IEnumerable<ProcedimentoDTO>>(procedimentos);
    }
}
