using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace Mk.DemoB.HttpApi.Client.ConsoleTestApp
{
    [DependsOn(
        typeof(DemoBHttpApiClientModule),
        typeof(AbpHttpClientIdentityModelModule)
        )]
    public class DemoBConsoleApiClientModule : AbpModule
    {
        
    }
}
