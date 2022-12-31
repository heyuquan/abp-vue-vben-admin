using Leopard.Base.Shared;
using Volo.Abp.Reflection;

namespace Leopard.BackendAdmin.Permissions
{
    public static class BackendAdminPermissions
    {
        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(BackendAdminPermissions));
        }

        public const string GroupName = ModuleNames.BackendAdmin;

        public static class Settings
        {
            public const string Default = GroupName + ".Settings";
        }
    }
}