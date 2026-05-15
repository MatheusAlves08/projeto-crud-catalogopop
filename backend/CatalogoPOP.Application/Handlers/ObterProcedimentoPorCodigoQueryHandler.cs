using CatalogoPOP.Application.Queries;
using CatalogoPOP.Domain.Interfaces;
using MediatR;

namespace CatalogoPOP.Application.Handlers;

public class ObterProcedimentoPorCodigoQueryHandler : IRequestHandler<ObterProcedimentoPorCodigoQuery, bool>
{
    private readonly IProcedimentoRepository _repository;

    public ObterProcedimentoPorCodigoQueryHandler(IProcedimentoRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(ObterProcedimentoPorCodigoQuery request, CancellationToken cancellationToken)
    {
        // Buscamos todos os procedimentos (incluindo deletados se necessário, mas aqui queremos saber se o CÓDIGO está em uso)
        var procedimentos = await _repository.ObterTodosAsync();
        return procedimentos.Any(p => p.Codigo.Equals(request.Codigo, StringComparison.OrdinalIgnoreCase));
    }
}
