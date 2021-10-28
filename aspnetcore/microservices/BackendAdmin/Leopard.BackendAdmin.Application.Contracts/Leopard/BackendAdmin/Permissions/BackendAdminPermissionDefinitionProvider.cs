using Leopard.BackendAdmin.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Leopard.BackendAdmin.Permissions
{
    public class BackendAdminPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(BackendAdminPermissions.GroupName);
            //Define your own permissions here. Example:
            //myGroup.AddPermission(BackendAdminPermissions.MyPermission1, L("Permission:MyPermission1"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<BackendAdminResource>(name);
        }
    }
}
