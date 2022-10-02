using System;

namespace Leopard.EntityFrameworkCore
{
    public static class LeopardDbProperties
    {
        public static string DbTablePrefix { get; set; } = "Leopard";

        public static string DbSchema { get; set; } = null;

        public const string DefaultDbConnectionStringName = "Default";
    }
}
