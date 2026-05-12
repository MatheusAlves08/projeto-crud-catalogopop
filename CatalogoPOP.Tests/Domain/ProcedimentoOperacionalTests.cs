using CatalogoPOP.Domain.Entities;
using CatalogoPOP.Domain.Enums;
using CatalogoPOP.Domain.Exceptions;
using FluentAssertions;

namespace CatalogoPOP.Tests.Domain;

/// <summary>
/// Testes unitários para a entidade ProcedimentoOperacional.
///
/// O que são Testes Unitários?
/// São testes que validam uma ÚNICA unidade de código (uma classe, um método)
/// de forma isolada, sem banco de dados, sem internet, sem nada externo.
///
/// Por que usamos FluentAssertions?
/// Porque ele deixa os testes muito mais legíveis. Em vez de:
///   Assert.Equal("1.1", procedimento.Versao);
/// Escrevemos:
///   procedimento.Versao.Should().Be("1.1");
/// Muito mais natural!
/// </summary>
public class ProcedimentoOperacionalTests
{
    // ============================================================
    // TESTES DE CRIAÇÃO (Construtor)
    // ============================================================

    [Fact]
    public void Criar_ComDadosValidos_DeveInicializarComStatusRascunho()
    {
        // ARRANGE: Preparamos os dados de entrada
        var codigo = "HIG-001";
        var titulo = "Higienização de Equipamentos";
        var descricao = "Procedimento de higienização padrão.";
        var responsavel = "João Silva";

        // ACT: Executamos a ação que queremos testar
        var procedimento = new ProcedimentoOperacional(
            codigo, titulo, descricao, Departamento.Producao, responsavel);

        // ASSERT: Verificamos que o resultado é o esperado
        procedimento.Status.Should().Be(StatusProcedimento.Rascunho,
            "um POP novo sempre começa como rascunho");
    }

    [Fact]
    public void Criar_ComDadosValidos_DeveInicializarComVersao1Ponto0()
    {
        var procedimento = CriarProcedimentoValido();

        procedimento.Versao.Should().Be("1.0",
            "a versão inicial de um POP deve ser sempre 1.0");
    }

    [Fact]
    public void Criar_ComDadosValidos_IsDeletedDeveSerFalso()
    {
        var procedimento = CriarProcedimentoValido();

        procedimento.IsDeleted.Should().BeFalse(
            "um POP recém-criado não deve estar marcado como excluído");
    }

    [Fact]
    public void Criar_ComDadosValidos_IdDeveSerGeradoAutomaticamente()
    {
        var procedimento = CriarProcedimentoValido();

        procedimento.Id.Should().NotBeEmpty(
            "um GUID vazio (all zeros) indicaria que o ID não foi gerado");
    }

    // ============================================================
    // TESTES DE VALIDAÇÃO (Regras de Negócio no Construtor)
    // ============================================================

    [Theory]
    [InlineData("")]        // string vazia
    [InlineData("  ")]      // apenas espaços
    [InlineData(null)]      // nulo
    public void Criar_ComCodigoInvalido_DeveLancarDomainException(string? codigoInvalido)
    {
        // Usamos 'FluentActions.Invoking' para capturar a exceção do construtor
        var acao = () => new ProcedimentoOperacional(
            codigoInvalido!, "Titulo", "Descricao", Departamento.Producao, "Responsavel");

        acao.Should().Throw<DomainException>()
            .WithMessage("*Código*",
                "o sistema deve rejeitar um código vazio com mensagem clara");
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(null)]
    public void Criar_ComTituloInvalido_DeveLancarDomainException(string? tituloInvalido)
    {
        var acao = () => new ProcedimentoOperacional(
            "COD-001", tituloInvalido!, "Descricao", Departamento.Producao, "Responsavel");

        acao.Should().Throw<DomainException>()
            .WithMessage("*Título*");
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(null)]
    public void Criar_ComDescricaoInvalida_DeveLancarDomainException(string? descricaoInvalida)
    {
        var acao = () => new ProcedimentoOperacional(
            "COD-001", "Titulo", descricaoInvalida!, Departamento.Producao, "Responsavel");

        acao.Should().Throw<DomainException>()
            .WithMessage("*Descrição*");
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(null)]
    public void Criar_ComResponsavelInvalido_DeveLancarDomainException(string? responsavelInvalido)
    {
        var acao = () => new ProcedimentoOperacional(
            "COD-001", "Titulo", "Descricao", Departamento.Producao, responsavelInvalido!);

        acao.Should().Throw<DomainException>()
            .WithMessage("*Responsável*");
    }

    // ============================================================
    // TESTES DE VERSIONAMENTO AUTOMÁTICO (Regra Principal do Sistema)
    // ============================================================

    [Fact]
    public void Atualizar_QuandoStatusMudaParaAprovado_DeveIncrementarVersao()
    {
        // ARRANGE
        var procedimento = CriarProcedimentoValido();
        procedimento.Versao.Should().Be("1.0"); // Garantimos que começa em 1.0

        // ACT: Atualizamos o status para "Aprovado"
        procedimento.Atualizar(
            procedimento.Titulo,
            procedimento.Descricao,
            procedimento.Departamento,
            StatusProcedimento.Aprovado, // <-- A mudança que ativa o versionamento
            procedimento.Responsavel
        );

        // ASSERT: A versão deve ter subido para 1.1
        procedimento.Versao.Should().Be("1.1",
            "ao ser aprovado pela primeira vez, o POP deve ir de v1.0 para v1.1");
    }

    [Fact]
    public void Atualizar_QuandoStatusMudaParaAprovadoNovamente_DeveIncrementarVersaoParaCadaAprovacao()
    {
        var procedimento = CriarProcedimentoValido();

        // Primeira aprovação: 1.0 -> 1.1
        procedimento.Atualizar("T", "D", Departamento.Producao, StatusProcedimento.Aprovado, "R");
        // Volta para revisão para poder ser aprovado de novo
        procedimento.Atualizar("T", "D", Departamento.Producao, StatusProcedimento.EmRevisao, "R");

        // Segunda aprovação: 1.1 -> 1.2
        procedimento.Atualizar("T", "D", Departamento.Producao, StatusProcedimento.Aprovado, "R");

        procedimento.Versao.Should().Be("1.2",
            "cada nova aprovação deve incrementar a versão");
    }

    [Fact]
    public void Atualizar_QuandoStatusNaoMudaParaAprovado_NaoDeveIncrementarVersao()
    {
        var procedimento = CriarProcedimentoValido();

        // Apenas muda o título, sem alterar o status para Aprovado
        procedimento.Atualizar(
            "Novo Titulo",
            procedimento.Descricao,
            procedimento.Departamento,
            StatusProcedimento.Rascunho, // Mantém como rascunho
            procedimento.Responsavel
        );

        procedimento.Versao.Should().Be("1.0",
            "a versão só deve mudar quando o POP é aprovado");
    }

    [Fact]
    public void Atualizar_QuandoJaEstaAprovadoEStatusContinuaAprovado_NaoDeveIncrementarVersao()
    {
        var procedimento = CriarProcedimentoValido();

        // Aprova uma primeira vez (1.0 -> 1.1)
        procedimento.Atualizar("T", "D", Departamento.Producao, StatusProcedimento.Aprovado, "R");
        procedimento.Versao.Should().Be("1.1");

        // Tenta "aprovar de novo" sem sair do estado Aprovado
        procedimento.Atualizar("T", "D", Departamento.Producao, StatusProcedimento.Aprovado, "R");

        procedimento.Versao.Should().Be("1.1",
            "a versão não deve incrementar se o POP já estava aprovado antes");
    }

    // ============================================================
    // TESTES DE SOFT DELETE (Exclusão Lógica)
    // ============================================================

    [Fact]
    public void Excluir_QuandoProcedimentoAtivo_DeveMarcarComoExcluido()
    {
        var procedimento = CriarProcedimentoValido();

        procedimento.Excluir();

        procedimento.IsDeleted.Should().BeTrue(
            "após chamar Excluir(), o POP deve ser marcado como deletado");
    }

    [Fact]
    public void Excluir_QuandoJaExcluido_DeveLancarDomainException()
    {
        var procedimento = CriarProcedimentoValido();
        procedimento.Excluir(); // Primeira exclusão (válida)

        // Segunda exclusão (deve falhar)
        var acao = () => procedimento.Excluir();

        acao.Should().Throw<DomainException>()
            .WithMessage("*já está excluído*",
                "não é possível excluir um POP que já foi excluído");
    }

    // ============================================================
    // MÉTODO AUXILIAR (Helper)
    // Criado aqui para não repetir o mesmo código em cada teste.
    // ============================================================

    /// <summary>
    /// Cria um ProcedimentoOperacional com dados válidos para usar nos testes.
    /// </summary>
    private static ProcedimentoOperacional CriarProcedimentoValido()
    {
        return new ProcedimentoOperacional(
            codigo: "HIG-001",
            titulo: "Higienização de Equipamentos",
            descricao: "Procedimento padrão de higienização.",
            departamento: Departamento.Producao,
            responsavel: "João Silva"
        );
    }
}
