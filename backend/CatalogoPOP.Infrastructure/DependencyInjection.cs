using CatalogoPOP.Domain.Interfaces;
using CatalogoPOP.Infrastructure.Persistence;
using CatalogoPOP.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CatalogoPOP.Infrastructure;

/// <summary>
/// Classe de configuração para injeção de dependência da camada de Infraestrutura.
/// Aqui registramos o contexto do banco de dados e os repositórios.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // 1. Configura o Entity Framework com o PostgreSQL
        // A string de conexão virá do arquivo appsettings.json
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));

        // 2. Registra o Repositório.
        // Usamos 'AddScoped' para que uma nova instância do repositório seja criada para cada requisição HTTP.
        // Isso garante que todos os comandos que usarem o banco na mesma "volta" da API usem a mesma conexão.
        services.AddScoped<IProcedimentoRepository, ProcedimentoRepositorySql>();

        return services;
    }
}
