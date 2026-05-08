namespace CatalogoPOP.Domain.Enums;

/// <summary>
/// Representa o ciclo de vida (status) de um Procedimento Operacional Padrão.
/// </summary>
public enum StatusProcedimento
{
    Rascunho,   // Quando o POP está sendo criado e ainda não tem validade
    EmRevisao,  // Quando está aguardando aprovação
    Aprovado,   // Quando é válido e pode ser utilizado pela equipe
    Obsoleto    // Quando não deve mais ser utilizado
}
