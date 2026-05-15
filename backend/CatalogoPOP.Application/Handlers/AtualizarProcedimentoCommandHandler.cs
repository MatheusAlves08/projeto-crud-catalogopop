using CatalogoPOP.Application.Commands;
using CatalogoPOP.Domain.Enums;
using CatalogoPOP.Domain.Interfaces;
using MediatR;

namespace CatalogoPOP.Application.Handlers;

public class AtualizarProcedimentoCommandHandler : IRequestHandler<AtualizarProcedimentoCommand, bool>
{
    private readonly IProcedimentoRepository _repository;

    public AtualizarProcedimentoCommandHandler(IProcedimentoRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(AtualizarProcedimentoCommand request, CancellationToken cancellationToken)
    {
        var pop = await _repository.ObterPorIdAsync(request.Id);
        if (pop == null) return false;

        if (!Enum.TryParse<StatusProcedimento>(request.Status, out var status))
        {
            status = pop.Status;
        }

        // Regras de negócio de atualização e versionamento centralizadas na entidade
        pop.Atualizar(
            request.Titulo,
            request.Descricao,
            (Departamento)request.Departamento,
            status,
            request.Responsavel
        );

        await _repository.AtualizarAsync(pop);
        return await _repository.UnitOfWorkAsync();
    }
}
