using CatalogoPOP.Domain.Entities;
using CatalogoPOP.Domain.Interfaces;
using CatalogoPOP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CatalogoPOP.Infrastructure.Repositories;

/// <summary>
/// A implementação real do nosso Repositório usando o Entity Framework e SQL (PostgreSQL).
/// Essa classe "conversa" com o ApplicationDbContext.
/// </summary>
public class ProcedimentoRepositorySql : IProcedimentoRepository
{
    private readonly ApplicationDbContext _context;

    // Recebemos o contexto de banco de dados via injeção de dependência
    public ProcedimentoRepositorySql(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProcedimentoOperacional>> ObterTodosAsync()
    {
        // Filtramos para não trazer procedimentos marcados como excluídos (Soft Delete)
        return await _context.Procedimentos
            .AsNoTracking()
            .Where(p => !p.IsDeleted)
            .ToListAsync();
    }

    public async Task<ProcedimentoOperacional?> ObterPorIdAsync(Guid id)
    {
        // Busca o procedimento pela chave primária, garantindo que não esteja excluído
        return await _context.Procedimentos.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
    }

    public async Task<bool> ExisteCodigoAsync(string codigo)
    {
        // Verifica rapidamente se existe algum registro com esse código (retorna true ou false)
        // Isso é usado para a nossa Regra de Anti-duplicidade
        return await _context.Procedimentos.AnyAsync(p => p.Codigo == codigo);
    }

    public async Task AdicionarAsync(ProcedimentoOperacional procedimento)
    {
        // Prepara o objeto para ser inserido no banco (ainda não salvou)
        await _context.Procedimentos.AddAsync(procedimento);
    }

    public Task AtualizarAsync(ProcedimentoOperacional procedimento)
    {
        // Prepara o objeto para ser atualizado no banco (ainda não salvou)
        _context.Procedimentos.Update(procedimento);
        return Task.CompletedTask;
    }

    public async Task<bool> UnitOfWorkAsync()
    {
        // Finalmente aplica (commita) as mudanças no banco de dados.
        // SaveChangesAsync retorna a quantidade de linhas afetadas.
        return await _context.SaveChangesAsync() > 0;
    }
}
