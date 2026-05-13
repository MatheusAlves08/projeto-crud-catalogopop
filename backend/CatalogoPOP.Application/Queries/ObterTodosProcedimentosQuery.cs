using CatalogoPOP.Application.DTOs;
using MediatR;

namespace CatalogoPOP.Application.Queries;

/// <summary>
/// Query para obter todos os procedimentos cadastrados.
/// No CQRS, Queries são usadas apenas para LEITURA. Elas não alteram nada no banco.
/// </summary>
public record ObterTodosProcedimentosQuery : IRequest<IEnumerable<ProcedimentoDTO>>;
