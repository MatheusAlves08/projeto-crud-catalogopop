namespace CatalogoPOP.Domain.Exceptions;

/// <summary>
/// Exceção personalizada para erros de regras de negócio (Domínio).
/// Isso evita que usemos exceções genéricas do sistema (como Exception ou ArgumentException),
/// deixando claro que o erro ocorreu porque uma regra do sistema foi violada.
/// </summary>
public class DomainException : Exception
{
    // O construtor recebe a mensagem de erro e repassa para a classe base (Exception)
    public DomainException(string message) : base(message)
    {
    }
}
