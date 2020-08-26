using Mk.DemoB.Dto.SaleOrders;
using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Threading;

namespace Mk.DemoB
{
    public static class DemoBDtoExtensions
    {
        private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

        public static void Configure()
        {
            OneTimeRunner.Run(() =>
            {
            /* You can add extension properties to DTOs
             * defined in the depended modules.
             *
             * Example:
             *
             * ObjectExtensionManager.Instance
             *   .AddOrUpdateProperty<IdentityRoleDto, string>("Title");
             *
             * See the documentation for more:
             * https://docs.abp.io/en/abp/latest/Object-Extensions
             */

            // 只有在默认值可能发生变化时(如示例中的DateTime.Now;) 才使用 DefaultValueFactory,
            // 如果是一个常量请使用 DefaultValue 选项.


            ObjectExtensionManager.Instance
                .AddOrUpdateProperty<SaleOrderDto, DateTime>("CustomerName");
                        
            });
        }
    }
}