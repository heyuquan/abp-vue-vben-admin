using Volo.Abp.Modularity;

namespace EShop.Identity;

[DependsOn(
    typeof(EShopIdentityApplicationModule),
    typeof(IdentityDomainTestModule)
    )]
public class IdentityApplicationTestModule : AbpModule
{

}
