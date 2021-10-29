using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectMapping;

namespace Leopard.Saas
{
    public class TenantStore : ITransientDependency, ITenantStore
	{
		protected ITenantRepository TenantRepository { get; }

		protected IObjectMapper<LeopardSaasDomainModule> ObjectMapper { get; }

		protected ICurrentTenant CurrentTenant { get; }

		public TenantStore(ITenantRepository tenantRepository, IObjectMapper<LeopardSaasDomainModule> objectMapper, ICurrentTenant currentTenant)
		{
			this.TenantRepository = tenantRepository;
			this.ObjectMapper = objectMapper;
			this.CurrentTenant = currentTenant;
		}

		public virtual async Task<TenantConfiguration> FindAsync(string name)
		{
			TenantConfiguration result;
			using (this.CurrentTenant.Change(null))
			{
				var tenant = await this.TenantRepository.FindByNameAsync(name);
				if (tenant == null)
				{
					result = null;
				}
				else
				{
					result = this.ObjectMapper.Map<Tenant, TenantConfiguration>(tenant);
				}
			}
			return result;
		}

		public virtual async Task<TenantConfiguration> FindAsync(Guid id)
		{
			TenantConfiguration result;
			using (this.CurrentTenant.Change(null))
			{
				Tenant tenant = await this.TenantRepository.FindAsync(id);
				if (tenant == null)
				{
					result = null;
				}
				else
				{
					result = this.ObjectMapper.Map<Tenant, TenantConfiguration>(tenant);
				}
			}
			return result;
		}

		public virtual TenantConfiguration Find(string name)
		{
			TenantConfiguration result;
			using (this.CurrentTenant.Change(null))
			{
				Tenant tenant = this.TenantRepository.FindByName(name);
				if (tenant == null)
				{
					result = null;
				}
				else
				{
					result = this.ObjectMapper.Map<Tenant, TenantConfiguration>(tenant);
				}
			}
			return result;
		}

		public virtual TenantConfiguration Find(Guid id)
		{
			TenantConfiguration result;
			using (this.CurrentTenant.Change(null))
			{
				Tenant tenant = this.TenantRepository.FindById(id);
				if (tenant == null)
				{
					result = null;
				}
				else
				{
					result = this.ObjectMapper.Map<Tenant, TenantConfiguration>(tenant);
				}
			}
			return result;
		}
	}
}
