using CatalogoPOP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CatalogoPOP.Infrastructure.Persistence;

/// <summary>
/// O "Coração" do Entity Framework. Essa classe é a representação do banco de dados na aplicação.
/// Ela gerencia a conexão, as tabelas (DbSets) e a criação do Schema (Migrações).
/// </summary>
public class ApplicationDbContext : DbContext
{
    // O construtor recebe as opções (como a string de conexão do PostgreSQL) da injeção de dependência (Program.cs)
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // DbSet representa uma tabela no banco de dados.
    public DbSet<ProcedimentoOperacional> Procedimentos { get; set; }

    // Este método é chamado na hora que o EF Core vai criar os modelos
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Aplica todas as configurações de mapeamento (como a que fizemos em ProcedimentoOperacionalConfiguration)
        // Isso lê a pasta atual e busca qualquer classe que herde de IEntityTypeConfiguration
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}
