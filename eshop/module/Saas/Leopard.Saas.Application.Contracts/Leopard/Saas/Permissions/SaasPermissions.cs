using Leopard.Base.Shared;
using Volo.Abp.Reflection;

namespace Leopard.Saas.Permissions
{
    public class SaasPermissions
    {
        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(SaasPermissions));
        }

        public const string GroupName = ModuleNames.Saas;

        public static class Tenants
        {
            public const string Default = GroupName + ".Tenants";

            public const string Create = Default + ".Create";

            public const string Update = Default + ".Update";

            public const string Delete = Default + ".Delete";

            public const string ManageFeatures = Default + ".ManageFeatures";

            public const string ManageConnectionStrings = Default + ".ManageConnectionStrings";

            public const string ViewChangeHistory = "AuditLogging.ViewChangeHistory:Volo.Saas.Tenant";
        }

        public static class Editions
        {
            public const string Default = GroupName + ".Editions";

            public const string Create = Default + ".Create";

            public const string Update = Default + ".Update";

            public const string Delete = Default + ".Delete";

            public const string ManageFeatures = Default + ".ManageFeatures";

            public const string ViewChangeHistory = "AuditLogging.ViewChangeHistory:Volo.Saas.Edition";
        }
    }
}
