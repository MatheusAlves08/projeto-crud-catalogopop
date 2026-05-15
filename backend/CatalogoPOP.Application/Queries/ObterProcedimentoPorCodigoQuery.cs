using MediatR;

namespace CatalogoPOP.Application.Queries;

public record ObterProcedimentoPorCodigoQuery(string Codigo) : IRequest<bool>;
