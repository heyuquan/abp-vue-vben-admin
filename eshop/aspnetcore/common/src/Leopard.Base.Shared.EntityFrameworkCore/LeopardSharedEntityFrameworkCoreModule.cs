﻿using System;
using Volo.Abp.Data;
using Volo.Abp.Modularity;

namespace EShop.Shared.EntityFrameworkCore
{
    public class LeopardSharedEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            ConfigureDatabaseConnections();
        }
        private void ConfigureDatabaseConnections()
        {
            Configure<AbpDbConnectionOptions>(options =>
            {
                //options.Databases.Configure("AdministrationService", database =>
                //{
                //    database.MappedConnections.Add("AbpAuditLogging");
                //    database.MappedConnections.Add("AbpPermissionManagement");
                //    database.MappedConnections.Add("AbpSettingManagement");
                //    database.MappedConnections.Add("AbpFeatureManagement");
                //    database.MappedConnections.Add("AbpBlobStoring");
                //});

                //options.Databases.Configure("IdentityService", database =>
                //{
                //    database.MappedConnections.Add("AbpIdentity");
                //    database.MappedConnections.Add("AbpIdentityServer");
                //});
            });
        }
    }
}
