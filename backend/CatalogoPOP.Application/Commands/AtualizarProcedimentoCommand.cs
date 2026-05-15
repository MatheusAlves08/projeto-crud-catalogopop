using MediatR;

namespace CatalogoPOP.Application.Commands;

/// <summary>
/// Comando para atualizar um procedimento. 
/// Usamos uma classe simples para garantir compatibilidade total com o JSON do Frontend.
/// </summary>
public class AtualizarProcedimentoCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public int Departamento { get; set; }
    public string Responsavel { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    
    // Incluímos estes campos para evitar erros de desserialização se o front enviá-los,
    // embora a lógica de versão seja controlada pelo domínio.
    public string? Versao { get; set; }
    public string? Codigo { get; set; }
}
