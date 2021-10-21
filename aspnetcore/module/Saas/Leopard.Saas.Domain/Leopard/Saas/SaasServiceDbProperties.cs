namespace Leopard.Saas
{
    public static class SaasServiceDbProperties
    {
        public static string DbTablePrefix { get; set; } = "";

        public static string DbSchema { get; set; } = null;

        public const string ConnectionStringName = "LeopardSaas";
    }
}
