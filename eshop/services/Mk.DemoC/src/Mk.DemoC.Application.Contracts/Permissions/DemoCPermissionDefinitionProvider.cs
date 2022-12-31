using Mk.DemoC.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Mk.DemoC.Permissions
{
    public class DemoCPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(DemoCPermissions.GroupName, L("Permission:DemoC"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<DemoCResource>(name);
        }
    }
}