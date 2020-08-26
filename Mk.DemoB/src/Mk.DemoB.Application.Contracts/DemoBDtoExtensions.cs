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
                // 订单时间
                ObjectExtensionManager.Instance
                    .AddOrUpdateProperty<DateTime>(
                        new[]
                        {
                            //typeof(CreateSaleOrderRequest),
                            typeof(SaleOrderDto)
                        },
                        "OrderTime"
                        , options =>
                        {
                            options.Attributes.Add(new RequiredAttribute());
                            // options.DefaultValueFactory = () => DateTime.Now;
                        }
                    )
                    .AddOrUpdateProperty<DateTime>(
                        new[]
                        {
                            //typeof(CreateSaleOrderRequest),
                            typeof(SaleOrderDto),
                            //typeof(GetSaleOrderPagingRequest),
                        },
                        "CustomerName"
                        , options =>
                        {
                            options.Attributes.Add(new RequiredAttribute());
                            options.Attributes.Add(new StringLengthAttribute(64));
                            options.Validators.Add(context =>
                            {
                                string customerName = context.Value as string;

                                if (string.Compare(customerName, "jet", true) == 0)
                                {
                                    context.ValidationErrors.Add(
                                        new ValidationResult("客户名不能为 jet")
                                        );
                                }
                            });
                        }
                    );
            });
        }
    }
}