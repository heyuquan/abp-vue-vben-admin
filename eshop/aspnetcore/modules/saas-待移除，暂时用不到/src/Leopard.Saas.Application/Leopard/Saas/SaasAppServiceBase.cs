﻿using Volo.Abp.Application.Services;
using Leopard.Saas.Localization;

namespace Leopard.Saas
{
    public abstract class SaasAppServiceBase : ApplicationService
	{
		protected SaasAppServiceBase()
		{
			base.ObjectMapperContext = typeof(LeopardSaasApplicationModule);
			base.LocalizationResource = typeof(SaasResource);
		}
	}
}
