﻿using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Account;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace SSO.AuthServer
{
    [DependsOn(
        typeof(AuthServerApplicationContractsModule),
        typeof(AbpAccountHttpApiClientModule),
        typeof(AbpIdentityHttpApiClientModule)
    )]
    public class AuthServerHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "AuthServer";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(AuthServerApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
