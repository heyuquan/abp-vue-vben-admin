using SSO.AuthServer.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SSO.AuthServer
{
    [DependsOn(
        typeof(AuthServerEntityFrameworkCoreTestModule)
        )]
    public class AuthServerDomainTestModule : AbpModule
    {

    }
}