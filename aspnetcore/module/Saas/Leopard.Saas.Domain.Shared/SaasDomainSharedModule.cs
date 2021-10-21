using Leopard.Saas.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Leopard.Saas
{
	public class SaasDomainSharedModule : AbpModule
	{
		public override void ConfigureServices(ServiceConfigurationContext context)
		{
			Configure<AbpVirtualFileSystemOptions>(options =>
			{
				options.FileSets.AddEmbedded<SaasDomainSharedModule>();
			});

			Configure<AbpLocalizationOptions>(options =>
			{
				options.Resources.Add<SaasResource>("en")
				.AddBaseTypes(typeof(AbpValidationResource))
				.AddVirtualJson("/Leopard/Saas/Localization/Resource");
			});

			Configure<AbpExceptionLocalizationOptions>(options =>
			{
				options.MapCodeNamespace("Leopard.Saas", typeof(SaasResource));
			});
		}
	}
}
