using Leopard.Buiness.Shared;
using Volo.Abp.Reflection;

namespace Leopard.Account.Admin
{
    public static class AccountPermissions
    {
        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(AccountPermissions));
        }

        public const string GroupName = ModuleNames.AccountAdmin;

        public const string SettingManagement = ModuleNames.AccountAdmin + ".SettingManagement";
    }
}
