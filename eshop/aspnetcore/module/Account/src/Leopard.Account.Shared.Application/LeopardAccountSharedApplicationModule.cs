using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Emailing;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;
using Leopard.Identity;

namespace Leopard.Account
{
    [DependsOn(
		typeof(AbpDddApplicationModule),
		typeof(AbpUiNavigationModule),
		typeof(AbpEmailingModule),
		typeof(AbpAutoMapperModule),
		typeof(LeopardIdentityApplicationModule),
		typeof(LeopardAccountSharedApplicationContractsModule)		
	)]
	public class LeopardAccountSharedApplicationModule : AbpModule
	{
		public override void ConfigureServices(ServiceConfigurationContext context)
		{
		}
	}
}
