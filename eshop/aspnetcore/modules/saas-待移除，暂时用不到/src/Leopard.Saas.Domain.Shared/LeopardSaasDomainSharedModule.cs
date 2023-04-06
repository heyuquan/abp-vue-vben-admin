using EShop.Shared;
using Leopard.Saas.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Leopard.Saas
{
	public class LeopardSaasDomainSharedModule : AbpModule
	{
		public override void ConfigureServices(ServiceConfigurationContext context)
		{
			Configure<AbpVirtualFileSystemOptions>(options =>
			{
				options.FileSets.AddEmbedded<LeopardSaasDomainSharedModule>();
			});

			Configure<AbpLocalizationOptions>(options =>
			{
				options.Resources.Add<SaasResource>()
				.AddBaseTypes(typeof(AbpValidationResource))
				.AddVirtualJson("/Leopard/Saas/Localization/Resources");
			});

			Configure<AbpExceptionLocalizationOptions>(options =>
			{
				options.MapCodeNamespace(ModuleNames.Saas, typeof(SaasResource));
			});
		}
	}
}
