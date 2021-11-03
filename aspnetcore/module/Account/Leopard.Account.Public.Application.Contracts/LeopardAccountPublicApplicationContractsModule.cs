﻿using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;
using Volo.Abp.Identity;
using Leopard.Identity;

namespace Leopard.Account.Public
{
    [DependsOn(
        typeof(LeopardAccountSharedApplicationContractsModule)
        )]
    public class LeopardAccountPublicApplicationContractsModule : AbpModule
    {

    }
}