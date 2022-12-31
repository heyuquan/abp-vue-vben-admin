using EShop.Common.Shared;
using Volo.Abp.Reflection;

namespace EShop.Administration.Permissions
{
    public static class AdministrationPermissions
    {
        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(AdministrationPermissions));
        }

        public const string GroupName = ModuleNames.Administration;

        public static class Settings
        {
            public const string Default = GroupName + ".Settings";
        }
    }
}