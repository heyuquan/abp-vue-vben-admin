﻿using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict;
using Volo.Abp.PermissionManagement;

namespace EShop.Identity;

[DependsOn(
    typeof(EShopIdentityDomainSharedModule),
    typeof(AbpOpenIddictDomainModule),
    typeof(AbpPermissionManagementDomainModule)
)]
public class EShopIdentityDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {

    }
}
