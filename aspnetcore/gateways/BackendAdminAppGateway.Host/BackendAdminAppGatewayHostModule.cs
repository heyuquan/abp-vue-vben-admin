using Leopard;
using Leopard.Account.Admin;
using Leopard.AspNetCore.Serilog;
using Leopard.AspNetCore.Swashbuckle;
using Leopard.BackendAdmin;
using Leopard.Consul;
using Leopard.Identity;
using Leopard.Saas;
using Leopard.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.HttpApi;
using Volo.Abp.SettingManagement;
using Volo.Abp.Swashbuckle;

namespace BackendAdminAppGateway.Host
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpSwashbuckleModule),
        typeof(LeopardAspNetCoreSerilogModule),
        typeof(LeopardConsulModule),
        typeof(LeopardAspNetCoreSwashbuckleModule),

        typeof(LeopardBackendAdminHttpApiModule)
    )]
    public class BackendAdminAppGatewayHostModule : GatewayCommonModule
    {
        public BackendAdminAppGatewayHostModule() : base(ApplicationServiceType.GateWay, "BackendAdminAppGateway", false)
        { }        
    }
}
