using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Data;
using Volo.Abp.Domain;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectExtending;
using Volo.Abp.ObjectExtending.Modularity;
using Leopard.Saas.Localization;
using EShop.Common.Shared;

namespace Leopard.Saas
{
	[DependsOn(
		typeof(AbpMultiTenancyModule),
		typeof(LeopardSaasDomainSharedModule),
		typeof(AbpDataModule),
		typeof(AbpDddDomainModule),
		typeof(AbpAutoMapperModule),
		typeof(AbpFeatureManagementDomainModule)
	)]
	public class LeopardSaasDomainModule : AbpModule
	{
		public override void ConfigureServices(ServiceConfigurationContext context)
		{
			Configure<FeatureManagementOptions>(options =>
			{
				options.ProviderPolicies["T"] = $"{ModuleNames.Saas}.Tenants.ManageFeatures";
				options.ProviderPolicies["E"] = $"{ModuleNames.Saas}.Editions.ManageFeatures";
			});

			context.Services.AddAutoMapperObjectMapper<LeopardSaasDomainModule>();

			Configure<AbpAutoMapperOptions>(options =>
			{
				options.AddProfile<SaasDomainMappingProfile>(true);
			});

			Configure<AbpDistributedEntityEventOptions>(options =>
			{
				options.EtoMappings.Add<Edition, EditionEto>(typeof(LeopardSaasDomainModule));
				options.EtoMappings.Add<Tenant, TenantEto>(typeof(LeopardSaasDomainModule));
			});

			Configure<AbpExceptionLocalizationOptions>(options =>
			{
				options.MapCodeNamespace(ModuleNames.Saas, typeof(SaasResource));
			});
		}

		public override void PostConfigureServices(ServiceConfigurationContext context)
		{
			ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToEntity(
				SaasModuleExtensionConsts.ModuleName,
				SaasModuleExtensionConsts.EntityNames.Tenant,
				typeof(Tenant)
			);
			ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToEntity(
				SaasModuleExtensionConsts.ModuleName,
				SaasModuleExtensionConsts.EntityNames.Edition,
				typeof(Edition)
			);
		}
	}
}

