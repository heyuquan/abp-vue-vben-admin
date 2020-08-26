using Mk.DemoB.Consts.SaleOrders;
using Mk.DemoB.SaleOrderMgr.Entities;
using System;
using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Threading;

namespace Mk.DemoB.EntityFrameworkCore
{
    public static class DemoBEfCoreEntityExtensionMappings
    {
        private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

        public static void Configure()
        {
            DemoBModulePropertyConfigurator.Configure();
            
            OneTimeRunner.Run(() =>
            {
                /* You can configure entity extension properties for the
                 * entities defined in the used modules.
                 *
                 * The properties defined here becomes table fields.
                 * If you want to use the ExtraProperties dictionary of the entity
                 * instead of creating a new field, then define the property in the
                 * DemoBDomainObjectExtensions class.
                 *
                 * Example:
                 *
                 * ObjectExtensionManager.Instance
                 *    .MapEfCoreProperty<IdentityUser, string>(
                 *        "MyProperty",
                 *        b => b.HasMaxLength(128)
                 *    );
                 *
                 * See the documentation for more:
                 * https://docs.abp.io/en/abp/latest/Customizing-Application-Modules-Extending-Entities
                 */

                ObjectExtensionManager.Instance
                    .AddOrUpdateProperty<SaleOrder, string>(
                        "CustomerName"
                        , options =>
                        {
                            options
                            .MapEfCore(b => b.IsRequired())
                            .MapEfCore(b => b.HasMaxLength(SaleOrderConsts.MaxCustomerNameLength));
                        }
                    );
            });
        }
    }
}
