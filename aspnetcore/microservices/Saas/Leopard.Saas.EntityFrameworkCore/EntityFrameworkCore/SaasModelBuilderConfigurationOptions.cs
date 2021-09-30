using JetBrains.Annotations;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Leopard.Saas.EntityFrameworkCore
{
    public class SaasModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public SaasModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}