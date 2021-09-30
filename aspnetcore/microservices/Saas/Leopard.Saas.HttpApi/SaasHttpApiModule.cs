﻿using Localization.Resources.AbpUi;
using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.FeatureManagement;
using Volo.Abp.FeatureManagement.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Leopard.Saas.Localization;

namespace Leopard.Saas
{
	[DependsOn(
		typeof(SaasApplicationContractsModule),
		typeof(AbpFeatureManagementHttpApiModule),
		typeof(AbpAspNetCoreMvcModule)
		)]
	public class SaasHttpApiModule : AbpModule
	{
		public override void PreConfigureServices(ServiceConfigurationContext context)
		{
			base.PreConfigure<IMvcBuilder>(builder =>
				builder.AddApplicationPartIfNotExists(typeof(SaasHttpApiModule).Assembly));
		}

		public override void ConfigureServices(ServiceConfigurationContext context)
		{
			base.Configure<AbpLocalizationOptions>(options =>
			{
				options.Resources.Get<SaasResource>().AddBaseTypes(new Type[]
				{
					typeof(AbpUiResource),
					typeof(AbpFeatureManagementResource)
				});
			});
		}
	}
}
