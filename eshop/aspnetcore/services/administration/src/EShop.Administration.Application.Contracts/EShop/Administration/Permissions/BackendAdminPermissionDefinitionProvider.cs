using EShop.Administration.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace EShop.Administration.Permissions
{
    public class AdministrationPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var settingGroup = context.AddGroup(AdministrationPermissions.GroupName, L("Permission:Settings"));
            var settingManagement = settingGroup.AddPermission(AdministrationPermissions.Settings.Default, L("Permission:SettingsManagement"), MultiTenancySides.Host);
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<AdministrationResource>(name);
        }
    }
}
