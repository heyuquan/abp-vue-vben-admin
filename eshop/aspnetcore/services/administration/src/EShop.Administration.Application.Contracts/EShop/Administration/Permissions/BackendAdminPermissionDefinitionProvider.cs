using EShop.Administration.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace EShop.Administration.Permissions
{
    public class BackendAdminPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var settingGroup = context.AddGroup(BackendAdminPermissions.GroupName, L("Permission:Settings"), MultiTenancySides.Host);
            var settingManagement = settingGroup.AddPermission(BackendAdminPermissions.Settings.Default, L("Permission:SettingsManagement"), MultiTenancySides.Host);
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<AdministrationResource>(name);
        }
    }
}
