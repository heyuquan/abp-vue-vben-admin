using Mk.DemoB.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Mk.DemoB.Permissions
{
    public class DemoBPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(DemoBPermissions.GroupName);

            //Define your own permissions here. Example:
            //myGroup.AddPermission(DemoBPermissions.MyPermission1, L("Permission:MyPermission1"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<DemoBResource>(name);
        }
    }
}
