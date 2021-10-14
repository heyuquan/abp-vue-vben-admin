using Volo.Abp.Reflection;

namespace Leopard.Account
{
    public static class AccountPermissions
	{
		public static string[] GetAll()
		{
			return ReflectionHelper.GetPublicConstantsRecursively(typeof(AccountPermissions));
		}

		public const string GroupName = "AccountAdmin";

		public const string SettingManagement = "AccountAdmin.SettingManagement";
	}
}
