using Volo.Abp.Modularity;

namespace SSO.AuthServer
{
    [DependsOn(
        typeof(AuthServerApplicationModule),
        typeof(AuthServerDomainTestModule)
        )]
    public class AuthServerApplicationTestModule : AbpModule
    {

    }
}