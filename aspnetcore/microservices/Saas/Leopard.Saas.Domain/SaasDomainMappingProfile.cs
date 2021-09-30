using AutoMapper;
using Volo.Abp.Data;
using Volo.Abp.MultiTenancy;

namespace Leopard.Saas
{
    public class SaasDomainMappingProfile : Profile
	{
		public SaasDomainMappingProfile()
		{
			CreateMap<Tenant, TenantConfiguration>()
				.ForMember(ti => ti.ConnectionStrings, opts =>
				{
					opts.MapFrom((tenant, ti) =>
					{
						var connectionStrings = new ConnectionStrings();

						foreach (var tenantConnectionString in tenant.ConnectionStrings)
						{
							connectionStrings[tenantConnectionString.Name] = tenantConnectionString.Value;
						}

						return connectionStrings;
					});
				})
				.ForMember(x => x.IsActive, x => x.Ignore());

			base.CreateMap<Edition, EditionEto>();
			base.CreateMap<Tenant, TenantEto>();
		}
	}
}
