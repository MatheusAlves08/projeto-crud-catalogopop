using CatalogoPOP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogoPOP.Infrastructure.Persistence.Configurations;

/// <summary>
/// Esta classe ensina o Entity Framework Core (EF Core) como salvar a entidade no banco de dados.
/// É a abordagem "Code First" usando "Fluent API", que é muito melhor do que sujar a entidade com anotações (Attributes).
/// </summary>
public class ProcedimentoOperacionalConfiguration : IEntityTypeConfiguration<ProcedimentoOperacional>
{
    public void Configure(EntityTypeBuilder<ProcedimentoOperacional> builder)
    {
        // Define o nome da tabela no PostgreSQL
        builder.ToTable("ProcedimentosOperacionais");

        // Define a Chave Primária (Primary Key)
        builder.HasKey(p => p.Id);

        // Regra de Duplicidade: Garante que o Código seja UNIQUE (Único no banco todo)
        // Isso impede, a nível de banco de dados, que dois procedimentos tenham o mesmo código (ex: "POP-001")
        builder.HasIndex(p => p.Codigo).IsUnique();

        // Limita o tamanho das strings para evitar gasto desnecessário de espaço no banco
        builder.Property(p => p.Codigo)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(p => p.Titulo)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(p => p.Descricao)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(p => p.Responsavel)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Versao)
            .IsRequired()
            .HasMaxLength(20);

        // Define que os Enums serão salvos como Strings no banco (Ex: "Aprovado" em vez de número 2)
        // Isso facilita muito na hora de ler direto no banco de dados.
        builder.Property(p => p.Departamento)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(p => p.Status)
            .HasConversion<string>()
            .IsRequired();

        // Filtro Global: Por padrão, o banco NUNCA vai retornar os registros que foram marcados como deletados (IsDeleted = true)
        // Isso implementa nosso "Soft Delete" em todas as consultas automaticamente.
        builder.HasQueryFilter(p => !p.IsDeleted);
    }
}
