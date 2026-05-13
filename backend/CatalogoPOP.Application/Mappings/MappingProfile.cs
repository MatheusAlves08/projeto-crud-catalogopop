using AutoMapper;
using CatalogoPOP.Application.DTOs;
using CatalogoPOP.Domain.Entities;

namespace CatalogoPOP.Application.Mappings;

/// <summary>
/// Perfil de mapeamento do AutoMapper.
/// O AutoMapper serve para "copiar" os dados de um objeto para outro automaticamente,
/// desde que os nomes das propriedades sejam iguais ou parecidos.
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Define que podemos transformar um 'ProcedimentoOperacional' em um 'ProcedimentoDTO'
        CreateMap<ProcedimentoOperacional, ProcedimentoDTO>();
    }
}
