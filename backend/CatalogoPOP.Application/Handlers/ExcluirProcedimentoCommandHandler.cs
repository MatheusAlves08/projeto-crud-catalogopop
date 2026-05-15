using CatalogoPOP.Application.Commands;
using CatalogoPOP.Domain.Interfaces;
using MediatR;

namespace CatalogoPOP.Application.Handlers;

public class ExcluirProcedimentoCommandHandler : IRequestHandler<ExcluirProcedimentoCommand, bool>
{
    private readonly IProcedimentoRepository _repository;

    public ExcluirProcedimentoCommandHandler(IProcedimentoRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(ExcluirProcedimentoCommand request, CancellationToken cancellationToken)
    {
        var pop = await _repository.ObterPorIdAsync(request.Id);
        if (pop == null) return false;

        pop.Excluir(); // Soft Delete (marcamos como IsDeleted = true)
        await _repository.AtualizarAsync(pop);
        return await _repository.UnitOfWorkAsync();
    }
}
