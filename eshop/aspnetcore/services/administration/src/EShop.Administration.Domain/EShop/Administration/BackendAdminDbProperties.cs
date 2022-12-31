using Volo.Abp.Data;

namespace EShop.Administration
{
    public static class BackendAdminDbProperties
    {
        public static string DbTablePrefix { get; set; } = AbpCommonDbProperties.DbTablePrefix;

        public static string DbSchema { get; set; } = AbpCommonDbProperties.DbSchema;

        public const string ConnectionStringName = "Default";
    }
}
