using AutoMapper;
using Leopard.Saas.Dtos;
using Volo.Abp.AutoMapper;

namespace Leopard.Saas
{
    public class SaasApplicationAutoMapperProfile : Profile
    {
        public SaasApplicationAutoMapperProfile()
        {
            CreateMap<Tenant, SaasTenantDto>().Ignore(x => x.EditionName);
            CreateMap<Edition, EditionDto>();
        }
    }
}