using Leopard.Saas.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;

namespace EShop.Administration.EntityFrameworkCore
{
    [DependsOn(
        typeof(EShopAdministrationDomainModule),
        typeof(AbpEntityFrameworkCoreMySQLModule),
        typeof(AbpPermissionManagementEntityFrameworkCoreModule),
        typeof(AbpSettingManagementEntityFrameworkCoreModule),       
        typeof(AbpAuditLoggingEntityFrameworkCoreModule),
        typeof(AbpFeatureManagementEntityFrameworkCoreModule),
        typeof(AbpIdentityEntityFrameworkCoreModule),
        typeof(LeopardSaasEntityFrameworkCoreModule)
        )]
    public class EShopAdministrationEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<AdministrationDbContext>(options =>
            {
                /* Remove "includeAllEntities: true" to create
                 * default repositories only for aggregate roots */
                options.AddDefaultRepositories(includeAllEntities: true);
            });

            Configure<AbpDbContextOptions>(options =>
            {
                /* The main point to change your DBMS.
                 * See also AdministrationMigrationsDbContextFactory for EF Core tooling. */
                options.UseMySQL();
            });
        }
    }
}
