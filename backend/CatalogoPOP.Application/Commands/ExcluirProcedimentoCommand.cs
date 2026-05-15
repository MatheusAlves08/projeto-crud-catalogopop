using MediatR;

namespace CatalogoPOP.Application.Commands;

public record ExcluirProcedimentoCommand(Guid Id) : IRequest<bool>;
