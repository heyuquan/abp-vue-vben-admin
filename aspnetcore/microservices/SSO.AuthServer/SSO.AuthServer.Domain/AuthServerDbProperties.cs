using Volo.Abp.Data;

namespace SSO.AuthServer
{
    public static class AuthServerDbProperties
    {
        public static string DbTablePrefix { get; set; } = AbpCommonDbProperties.DbTablePrefix;

        public static string DbSchema { get; set; } = AbpCommonDbProperties.DbSchema;

        public const string ConnectionStringName = "LeopardSSOAuthServer";
    }
}
