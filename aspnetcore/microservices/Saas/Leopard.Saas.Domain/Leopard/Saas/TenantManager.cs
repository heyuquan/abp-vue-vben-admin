using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Services;

namespace Leopard.Saas
{
    public class TenantManager : DomainService, ITenantManager, ITransientDependency, IDomainService
	{
		protected ITenantRepository TenantRepository { get; }

		public TenantManager(ITenantRepository tenantRepository)
		{
			this.TenantRepository = tenantRepository;
		}

		public virtual async Task<Tenant> CreateAsync(string name, Guid? editionId = null)
		{
			Check.NotNull<string>(name, nameof(name));
			await this.ValidateNameAsync(name);
			return new Tenant(base.GuidGenerator.Create(), name, editionId);
		}

		public virtual async Task ChangeNameAsync(Tenant tenant, string name)
		{
			Check.NotNull<Tenant>(tenant, nameof(tenant));
			Check.NotNull<string>(name, nameof(name));
			await this.ValidateNameAsync(name, new Guid?(tenant.Id));
			tenant.SetName(name);
		}

		protected virtual async Task ValidateNameAsync(string name, Guid? expectedId = null)
		{
			Tenant tenant = await this.TenantRepository.FindByNameAsync(name);
			if (tenant != null && tenant.Id != expectedId)
			{
				throw new BusinessException("Yun.Saas:DuplicateTenantName").WithData("Name", name);
			}
		}
	}
}
