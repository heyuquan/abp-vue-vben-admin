﻿using Leopard.Account.Admin;
using Leopard.Identity;
using Leopard.Saas;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;

namespace Leopard.BackendAdmin
{
    [DependsOn(
        typeof(BackendAdminDomainSharedModule),
        typeof(AbpFeatureManagementApplicationContractsModule),
        typeof(AbpPermissionManagementApplicationContractsModule),
        typeof(AbpSettingManagementApplicationContractsModule),
        typeof(AbpObjectExtendingModule),
        typeof(SaasApplicationContractsModule),
        typeof(LeopardAccountAdminApplicationContractsModule),
        typeof(LeopardIdentityApplicationContractsModule)
    )]
    public class BackendAdminApplicationContractsModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            BackendAdminDtoExtensions.Configure();
        }
    }
}
