using Leopard.Saas.Permissions;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Auditing;
using Volo.Abp.Authorization.Permissions;

namespace Leopard.Saas
{
    public class SaasPermissionDefinitionProviderEntityHistoryTuner : PermissionDefinitionProvider
	{
		public override void Define(IPermissionDefinitionContext context)
		{
			var service = context.ServiceProvider.GetRequiredService<IAuditingHelper>();

			if (!service.IsEntityHistoryEnabled(typeof(Tenant)))
			{
				context.TryDisablePermission(SaasPermissions.Tenants.ViewChangeHistory);
			}

			if (!service.IsEntityHistoryEnabled(typeof(Edition)))
			{
				context.TryDisablePermission(SaasPermissions.Editions.ViewChangeHistory);
			}
		}
	}
}
