using Mk.DemoB.SaleOrderMgr.Entities;
using System;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Threading;

namespace Mk.DemoB.ObjectExtending
{
    public static class DemoBDomainObjectExtensions
    {
        private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

        public static void Configure()
        {
            OneTimeRunner.Run(() =>
            {
                /* You can configure extension properties to entities or other object types
                 * defined in the depended modules.
                 * 
                 * If you are using EF Core and want to map the entity extension properties to new
                 * table fields in the database, then configure them in the DemoBEfCoreEntityExtensionMappings
                 *
                 * Example:
                 *
                 * ObjectExtensionManager.Instance
                 *    .AddOrUpdateProperty<IdentityRole, string>("Title");
                 *
                 * See the documentation for more:
                 * https://docs.abp.io/en/abp/latest/Object-Extensions
                 */

                ObjectExtensionManager.Instance
                    .AddOrUpdateProperty<SaleOrder, string>("CustomerName")

                    // 虽然 CustomerName2 没有映射到独立的数据表字段，但是扩展字段依然要加到Dto上，
                    // 否则在对象映射 MapExtraProperties() 时，并不会把 CustomerName2 映射到目的对象的 ExtraProperties 字典中
                    .AddOrUpdateProperty<SaleOrder, string>("CustomerName2");
            });
        }
    }
}