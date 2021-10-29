using Leopard.Saas.Dtos;
using Leopard.Saas.Localization;
using System;
using Volo.Abp.Application;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.ObjectExtending.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Leopard.Saas
{
    [DependsOn(
		typeof(LeopardSaasDomainSharedModule),
		typeof(AbpDddApplicationModule)
	)]
	public class LeopardSaasApplicationContractsModule : AbpModule
	{
		public override void ConfigureServices(ServiceConfigurationContext context)
		{
			Configure<AbpVirtualFileSystemOptions>(options =>
			{
				options.FileSets.AddEmbedded<LeopardSaasApplicationContractsModule>();
			});

			Configure<AbpLocalizationOptions>(options =>
			{
				options.Resources.Get<SaasResource>().AddVirtualJson("/Leopard/Saas/Localization");
			});
		}

		public override void PostConfigureServices(ServiceConfigurationContext context)
		{
			ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToApi(
				SaasModuleExtensionConsts.ModuleName,
				SaasModuleExtensionConsts.EntityNames.Tenant,
				new Type[]
				{
					typeof(SaasTenantDto)
				}, 
				new Type[]
				{
					typeof(SaasTenantCreateDto)
				},
				new Type[]
				{
					typeof(SaasTenantUpdateDto)
				});

			ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToApi(
				SaasModuleExtensionConsts.ModuleName, 
				SaasModuleExtensionConsts.EntityNames.Edition, 
				new Type[]
				{
					typeof(EditionDto)
				}, 
				new Type[]
				{
					typeof(EditionCreateDto)
				}, 
				new Type[]
				{
					typeof(EditionUpdateDto)
				});
		}
	}
}
