using Leopard.UI.Navigation;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Identity.Localization;
using Volo.Abp.UI.Navigation;

namespace Leopard.Identity.Web.Navigation
{
    public class LeopardIdentityWebMenuContributor : IMenuContributor
    {
        public virtual Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name != LeopardStandardMenus.Main)
            {
                return Task.CompletedTask;
            }

            var l = context.GetLocalizer<IdentityResource>();

            var identityMenuItem = new ApplicationMenuItem(IdentityMenuNames.GroupName, l["Menu:IdentityManagement"], icon: "fa fa-id-card-o");
            identityMenuItem.AddItem(new ApplicationMenuItem(IdentityMenuNames.Roles, l["Roles"], url: "~/Identity/Roles").RequirePermissions(IdentityPermissions.Roles.Default));
            identityMenuItem.AddItem(new ApplicationMenuItem(IdentityMenuNames.Users, l["Users"], url: "~/Identity/Users").RequirePermissions(IdentityPermissions.Users.Default));

            context.Menu.GetAdministration().AddItem(identityMenuItem);

            return Task.CompletedTask;
        }
    }
}
