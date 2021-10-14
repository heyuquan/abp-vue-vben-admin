using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Auditing;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Identity;

namespace Leopard.Identity
{
	public class IdentityPermissionDefinitionProviderEntityHistoryTuner : PermissionDefinitionProvider
	{
		public override void Define(IPermissionDefinitionContext context)
		{
			IAuditingHelper requiredService = context.ServiceProvider.GetRequiredService<IAuditingHelper>();
			if (!requiredService.IsEntityHistoryEnabled(typeof(IdentityUser)))
			{
				context.TryDisablePermission("AuditLogging.ViewChangeHistory:Volo.Abp.Identity.IdentityUser");
			}
			if (!requiredService.IsEntityHistoryEnabled(typeof(IdentityRole)))
			{
				context.TryDisablePermission("AuditLogging.ViewChangeHistory:Volo.Abp.Identity.IdentityRole");
			}
		}
	}
}
