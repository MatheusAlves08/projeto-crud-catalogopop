using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogoPOP.Infrastructure.Migrations
{
    /// <summary>
    /// Migration InitialCreate: Este arquivo é gerado AUTOMATICAMENTE pelo comando 'dotnet ef migrations add'.
    ///
    /// O que é uma Migration?
    /// É um "script de banco de dados" escrito em C#. Em vez de escrever SQL na mão,
    /// o Entity Framework lê nossa classe (ProcedimentoOperacional) e gera este arquivo automaticamente.
    ///
    /// Como aplicar no banco de dados?
    /// Execute: dotnet ef database update
    /// Isso irá criar a tabela 'ProcedimentosOperacionais' no seu PostgreSQL.
    /// </summary>
    public partial class InitialCreate : Migration
    {
        /// <summary>
        /// Up(): Executa quando você roda 'dotnet ef database update'.
        /// Contém as instruções para CRIAR as tabelas.
        /// </summary>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Cria a tabela principal do sistema com todos os campos mapeados na entidade.
            migrationBuilder.CreateTable(
                name: "ProcedimentosOperacionais",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Codigo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Titulo = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Descricao = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Versao = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Departamento = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    Responsavel = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DataRevisao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false) // Campo do Soft Delete
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcedimentosOperacionais", x => x.Id);
                });

            // Cria um índice único no campo 'Codigo' para garantir que não haverá duplicatas.
            // Isso espelha a regra de negócio Anti-Duplicidade que definimos no repositório.
            migrationBuilder.CreateIndex(
                name: "IX_ProcedimentosOperacionais_Codigo",
                table: "ProcedimentosOperacionais",
                column: "Codigo",
                unique: true);
        }

        /// <summary>
        /// Down(): Executa quando você roda 'dotnet ef migrations remove' ou faz um rollback.
        /// Contém as instruções para DESFAZER o que o método Up() fez.
        /// </summary>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Apaga a tabela completamente (rollback total desta migration).
            migrationBuilder.DropTable(
                name: "ProcedimentosOperacionais");
        }
    }
}
