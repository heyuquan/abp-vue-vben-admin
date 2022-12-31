using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;
using Leopard.Account.Localization;

namespace Leopard.Account.Admin
{
	public class AccountPermissionDefinitionProvider : PermissionDefinitionProvider
	{
		public override void Define(IPermissionDefinitionContext context)
		{
			context.AddGroup(AccountPermissions.GroupName, L("Permission:Account"), MultiTenancySides.Both).AddPermission(AccountPermissions.SettingManagement, L("Permission:SettingManagement"), MultiTenancySides.Both);
		}

		private static LocalizableString L(string name)
		{
			return LocalizableString.Create<LeopardAccountResource>(name);
		}
	}
}
