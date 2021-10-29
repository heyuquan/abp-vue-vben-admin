using Leopard.Buiness.Shared;

namespace Leopard.Identity.Web.Navigation
{
    public class BackendAdminMenuNames
    {
        public class Identity
        {
            public const string GroupName = ModuleNames.Identity;

            public const string Roles = GroupName + ".Roles";
            public const string Users = GroupName + ".Users";
            public const string OrganizationUnits = GroupName + ".OrganizationUnits";
            public const string ClaimTypes = GroupName + ".ClaimTypes";
            public const string SecurityLog = GroupName + ".SecurityLog";
        }

        public class Saas
        {
            public const string GroupName = ModuleNames.Saas;

            public const string Tenants = GroupName + ".Tenants";
            public const string Editions = GroupName + ".Editions";
        }

        public class Settings
        {
            public const string GroupName = "Settings";
        }
    }
}
