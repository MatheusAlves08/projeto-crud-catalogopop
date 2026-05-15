using CatalogoPOP.Application.DTOs;
using MediatR;

namespace CatalogoPOP.Application.Queries;

public record ObterProcedimentoPorIdQuery(Guid Id) : IRequest<ProcedimentoDTO?>;
