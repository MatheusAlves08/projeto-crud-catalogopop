using CatalogoPOP.Application.DTOs;
using CatalogoPOP.Domain.Enums;
using MediatR;

namespace CatalogoPOP.Application.Commands;

/// <summary>
/// Representa o "Pedido" de criação de um novo procedimento.
/// No padrão CQRS (usando MediatR), o Command é uma classe que carrega os dados da intenção.
/// Implementa IRequest<ProcedimentoDTO>, o que significa que, após processado, retorna um DTO.
/// </summary>
public record CriarProcedimentoCommand : IRequest<ProcedimentoDTO>
{
    public string Codigo { get; init; } = string.Empty;
    public string Titulo { get; init; } = string.Empty;
    public string Descricao { get; init; } = string.Empty;
    public Departamento Departamento { get; init; }
    public string Responsavel { get; init; } = string.Empty;
}
