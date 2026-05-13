using CatalogoPOP.Domain.Enums;

namespace CatalogoPOP.Application.DTOs;

/// <summary>
/// DTO (Data Transfer Object) - Objeto de Transferência de Dados.
/// Diferente da Entidade (que tem lógica de negócio), o DTO serve apenas para 
/// levar dados de um lado para o outro (da API para o usuário, por exemplo).
/// Isso evita expor detalhes internos da nossa Entidade de Domínio.
/// </summary>
public record ProcedimentoDTO
{
    public Guid Id { get; init; }
    public string Codigo { get; init; } = string.Empty;
    public string Titulo { get; init; } = string.Empty;
    public string Descricao { get; init; } = string.Empty;
    public string Versao { get; init; } = string.Empty;
    public Departamento Departamento { get; init; }
    public StatusProcedimento Status { get; init; }
    public string Responsavel { get; init; } = string.Empty;
    public DateTime? DataRevisao { get; init; }
}
