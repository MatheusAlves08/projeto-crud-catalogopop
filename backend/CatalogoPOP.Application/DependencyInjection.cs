using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CatalogoPOP.Application;

/// <summary>
/// Classe de configuração para injeção de dependência da camada de Aplicação.
/// Centraliza o registro de bibliotecas como MediatR, AutoMapper e FluentValidation.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Obtém o Assembly (Projeto) atual para registrar automaticamente Handlers, Profiles e Validators
        var assembly = Assembly.GetExecutingAssembly();

        // Registra o MediatR e todos os seus Handlers
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

        // Registra o AutoMapper e todos os seus perfis de mapeamento (MappingProfiles)
        services.AddAutoMapper(assembly);

        // Registra o FluentValidation e todos os validadores
        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}
