using AutoMapper;
using CatalogoPOP.Application.Commands;
using CatalogoPOP.Application.DTOs;
using CatalogoPOP.Domain.Entities;
using CatalogoPOP.Domain.Exceptions;
using CatalogoPOP.Domain.Interfaces;
using MediatR;

namespace CatalogoPOP.Application.Handlers;

/// <summary>
/// O Handler (Manipulador) é quem realmente executa a lógica do comando.
/// Ele recebe o 'CriarProcedimentoCommand', faz as verificações necessárias e salva no banco.
/// </summary>
public class CriarProcedimentoCommandHandler : IRequestHandler<CriarProcedimentoCommand, ProcedimentoDTO>
{
    private readonly IProcedimentoRepository _repository;
    private readonly IMapper _mapper;

    // Injeção de Dependência: Pedimos o repositório e o mapeador que o .NET gerencia para nós.
    public CriarProcedimentoCommandHandler(IProcedimentoRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ProcedimentoDTO> Handle(CriarProcedimentoCommand request, CancellationToken cancellationToken)
    {
        // 1. Regra de Negócio: Anti-duplicidade
        // Antes de criar, verificamos se o código (Ex: POP-001) já existe no banco.
        if (await _repository.ExisteCodigoAsync(request.Codigo))
        {
            throw new DomainException($"Já existe um procedimento cadastrado com o código {request.Codigo}.");
        }

        // 2. Criação da Entidade
        // Chamamos o construtor da nossa entidade de domínio que já faz as validações internas.
        var procedimento = new ProcedimentoOperacional(
            request.Codigo,
            request.Titulo,
            request.Descricao,
            request.Departamento,
            request.Responsavel
        );

        // 3. Persistência
        // Adicionamos ao repositório e chamamos o UnitOfWork para salvar definitivamente no PostgreSQL.
        await _repository.AdicionarAsync(procedimento);
        await _repository.UnitOfWorkAsync();

        // 4. Retorno
        // Transformamos a entidade salva em um DTO para devolver para a API/Usuário.
        return _mapper.Map<ProcedimentoDTO>(procedimento);
    }
}
