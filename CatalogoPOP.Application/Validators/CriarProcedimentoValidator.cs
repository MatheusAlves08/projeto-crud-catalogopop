using CatalogoPOP.Application.Commands;
using FluentValidation;

namespace CatalogoPOP.Application.Validators;

/// <summary>
/// Validador usando FluentValidation. 
/// Define regras de preenchimento para o comando de criação.
/// O FluentValidation permite escrever regras de forma quase "humana" (fluente).
/// </summary>
public class CriarProcedimentoValidator : AbstractValidator<CriarProcedimentoCommand>
{
    public CriarProcedimentoValidator()
    {
        RuleFor(c => c.Codigo)
            .NotEmpty().WithMessage("O Código do POP é obrigatório.")
            .MaximumLength(20).WithMessage("O Código deve ter no máximo 20 caracteres.");

        RuleFor(c => c.Titulo)
            .NotEmpty().WithMessage("O Título é obrigatório.")
            .MinimumLength(5).WithMessage("O Título deve ter pelo menos 5 caracteres.")
            .MaximumLength(150).WithMessage("O Título deve ter no máximo 150 caracteres.");

        RuleFor(c => c.Descricao)
            .NotEmpty().WithMessage("A Descrição detalhada é obrigatória.")
            .MaximumLength(1000).WithMessage("A Descrição deve ter no máximo 1000 caracteres.");

        RuleFor(c => c.Responsavel)
            .NotEmpty().WithMessage("O nome do Responsável é obrigatório.")
            .MaximumLength(100).WithMessage("O nome do Responsável deve ter no máximo 100 caracteres.");

        RuleFor(c => c.Departamento)
            .IsInEnum().WithMessage("Departamento inválido.");
    }
}
