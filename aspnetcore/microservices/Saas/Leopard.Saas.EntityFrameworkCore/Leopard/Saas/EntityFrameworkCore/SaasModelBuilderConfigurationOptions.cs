using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Leopard.Saas.EntityFrameworkCore
{
    public class SaasModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
	{
		public SaasModelBuilderConfigurationOptions(string tablePrefix = "", string schema = null)
			: base(tablePrefix, schema)
		{
		}
	}
}
