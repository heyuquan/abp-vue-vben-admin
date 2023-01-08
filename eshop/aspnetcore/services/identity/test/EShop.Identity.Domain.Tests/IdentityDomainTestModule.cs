using EShop.Identity.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace EShop.Identity;

[DependsOn(
    typeof(IdentityEntityFrameworkCoreTestModule)
    )]
public class IdentityDomainTestModule : AbpModule
{

}
