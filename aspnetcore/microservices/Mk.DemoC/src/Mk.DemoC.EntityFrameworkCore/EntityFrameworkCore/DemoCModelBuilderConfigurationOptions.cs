using JetBrains.Annotations;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Mk.DemoC.EntityFrameworkCore
{
    public class DemoCModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public DemoCModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}