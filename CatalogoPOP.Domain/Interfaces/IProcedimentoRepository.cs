using CatalogoPOP.Domain.Entities;

namespace CatalogoPOP.Domain.Interfaces;

/// <summary>
/// Interface do Repositório. Ela define O QUE o repositório deve fazer,
/// mas não COMO ele faz. Quem vai dizer "como" é a camada de Infrastructure (onde fica o banco).
/// Isso permite que o resto do sistema converse com o banco sem depender do Entity Framework diretamente.
/// </summary>
public interface IProcedimentoRepository
{
    // Retorna todos os procedimentos, com paginação básica e sem os excluídos
    Task<IEnumerable<ProcedimentoOperacional>> ObterTodosAsync();
    
    // Retorna um procedimento pelo seu Id
    Task<ProcedimentoOperacional?> ObterPorIdAsync(Guid id);
    
    // Verifica se já existe um procedimento com esse código (Regra de Anti-duplicidade)
    Task<bool> ExisteCodigoAsync(string codigo);
    
    // Salva um novo procedimento no banco
    Task AdicionarAsync(ProcedimentoOperacional procedimento);
    
    // Atualiza um procedimento existente
    Task AtualizarAsync(ProcedimentoOperacional procedimento);
    
    // Commita (Salva) as alterações no banco de dados (Unit of Work implícito)
    Task<bool> UnitOfWorkAsync();
}
