using CatalogoPOP.Domain.Enums;
using CatalogoPOP.Domain.Exceptions;

namespace CatalogoPOP.Domain.Entities;

/// <summary>
/// A entidade principal do nosso sistema. Representa um POP (Procedimento Operacional Padrão).
/// Em Clean Architecture, as entidades contêm não apenas os dados, mas também as regras de negócio.
/// </summary>
public class ProcedimentoOperacional
{
    // Propriedades com 'private set' garantem que elas só possam ser modificadas
    // através de métodos definidos dentro desta própria classe (Encapsulamento).
    // Isso impede que outras partes do sistema mudem os dados de forma inválida.
    public Guid Id { get; private set; }
    public string Codigo { get; private set; }
    public string Titulo { get; private set; }
    public string Descricao { get; private set; }
    public string Versao { get; private set; }
    public Departamento Departamento { get; private set; }
    public StatusProcedimento Status { get; private set; }
    public string Responsavel { get; private set; }
    public DateTime? DataRevisao { get; private set; }
    
    // Hard Delete Protection (Soft Delete): Em vez de apagar do banco, marcamos como excluído.
    public bool IsDeleted { get; private set; }

    // Construtor vazio exigido pelo Entity Framework Core para conseguir criar o objeto
    // ao ler do banco de dados. Usamos '#pragma warning disable' apenas para ocultar um aviso de compilação.
#pragma warning disable CS8618
    protected ProcedimentoOperacional() { }
#pragma warning restore CS8618

    /// <summary>
    /// Construtor usado quando queremos criar um NOVO Procedimento na aplicação.
    /// Ele já inicializa com os valores padrão (Ex: Versão 1.0, Status Rascunho).
    /// </summary>
    public ProcedimentoOperacional(string codigo, string titulo, string descricao, Departamento departamento, string responsavel)
    {
        Validar(codigo, titulo, descricao, responsavel);

        Id = Guid.NewGuid();
        Codigo = codigo;
        Titulo = titulo;
        Descricao = descricao;
        Versao = "1.0"; // Versão inicial
        Departamento = departamento;
        Status = StatusProcedimento.Rascunho; // Status inicial
        Responsavel = responsavel;
        IsDeleted = false;
    }

    /// <summary>
    /// Método para atualizar os dados do Procedimento. 
    /// Aqui também aplicamos a regra de versionamento automático.
    /// </summary>
    public void Atualizar(string titulo, string descricao, Departamento departamento, StatusProcedimento status, string responsavel)
    {
        Validar(Codigo, titulo, descricao, responsavel);

        Titulo = titulo;
        Descricao = descricao;
        Departamento = departamento;
        Responsavel = responsavel;

        // Se o status for aprovado, incrementa a versão e atualiza a data de revisão
        if (status == StatusProcedimento.Aprovado && Status != StatusProcedimento.Aprovado)
        {
            Status = status;
            IncrementarVersao();
            DataRevisao = DateTime.UtcNow;
        }
        else
        {
            Status = status;
        }
    }

    /// <summary>
    /// Aplica o "Soft Delete" (Exclusão Lógica). 
    /// Protege contra perda de dados apagando apenas de forma virtual.
    /// </summary>
    public void Excluir()
    {
        if (IsDeleted)
            throw new DomainException("Procedimento já está excluído.");
        
        IsDeleted = true;
    }

    /// <summary>
    /// Regra de Negócio: Se a versão for v1.0, ela vai para v1.1. 
    /// Quebra a string no ponto '.' e incrementa a parte secundária.
    /// </summary>
    private void IncrementarVersao()
    {
        var versaoParts = Versao.Split('.');
        if (versaoParts.Length == 2 && int.TryParse(versaoParts[1], out int minorVersion))
        {
            Versao = $"{versaoParts[0]}.{minorVersion + 1}";
        }
        else
        {
            Versao = "1.1";
        }
    }

    /// <summary>
    /// Validações básicas (Domain Validations) para garantir que não vamos
    /// salvar nada em branco. Lança uma DomainException se algo estiver errado.
    /// </summary>
    private void Validar(string codigo, string titulo, string descricao, string responsavel)
    {
        if (string.IsNullOrWhiteSpace(codigo))
            throw new DomainException("Código é obrigatório.");
        
        if (string.IsNullOrWhiteSpace(titulo))
            throw new DomainException("Título é obrigatório.");

        if (string.IsNullOrWhiteSpace(descricao))
            throw new DomainException("Descrição é obrigatória.");

        if (string.IsNullOrWhiteSpace(responsavel))
            throw new DomainException("Responsável é obrigatório.");
    }
}
