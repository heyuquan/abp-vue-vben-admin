using Leopard.BackendAdmin.Localization;
using Leopard.BackendAdmin.Permissions;
using Leopard.Saas.Localization;
using Leopard.Saas.Permissions;
using Leopard.UI.Navigation;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Identity.Localization;
using Volo.Abp.UI.Navigation;

namespace Leopard.Identity.Web.Navigation
{
    public class BackendAdminWebMenuContributor : IMenuContributor
    {
        public virtual Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name != LeopardStandardMenus.Main)
            {
                return Task.CompletedTask;
            }

            var backendAdminLocalizer = context.GetLocalizer<BackendAdminResource>();

            // identity
            var identityLocalizer = context.GetLocalizer<IdentityResource>();

            var identityMenuItem = new ApplicationMenuItem(BackendAdminMenuNames.Identity.GroupName, identityLocalizer["Menu:IdentityManagement"], icon: "fa fa-id-card-o");
            identityMenuItem.AddItem(new ApplicationMenuItem(BackendAdminMenuNames.Identity.OrganizationUnits, identityLocalizer["Menu:OrganizationUnits"], url: "/identity/organization-units")
                .RequirePermissions(IdentityPermissions.OrganizationUnits.Default));
            identityMenuItem.AddItem(new ApplicationMenuItem(BackendAdminMenuNames.Identity.Roles, identityLocalizer["Menu:Roles"], url: "/identity/roles")
                .RequirePermissions(IdentityPermissions.Roles.Default));
            identityMenuItem.AddItem(new ApplicationMenuItem(BackendAdminMenuNames.Identity.Users, identityLocalizer["Menu:Users"], url: "/identity/users")
                .RequirePermissions(IdentityPermissions.Users.Default));
            identityMenuItem.AddItem(new ApplicationMenuItem(BackendAdminMenuNames.Identity.ClaimTypes, identityLocalizer["Menu:ClaimTypes"], url: "/identity/claim-types")
                .RequirePermissions(IdentityPermissions.ClaimTypes.Default));
            identityMenuItem.AddItem(new ApplicationMenuItem(BackendAdminMenuNames.Identity.SecurityLog, identityLocalizer["Menu:SecurityLog"], url: "/identity/claim-types")
                .RequirePermissions(IdentityPermissions.SecurityLog.Default));

            // settings            
            identityMenuItem.AddItem(new ApplicationMenuItem(BackendAdminMenuNames.Settings.GroupName, backendAdminLocalizer["Menu:Settings"], url: "/setting-management")
                .RequirePermissions(BackendAdminPermissions.Settings.Default));

            context.Menu.GetAdministration().AddItem(identityMenuItem);

            // saas
            var saasLocalizer = context.GetLocalizer<SaasResource>();
            var saasMenuItem = new ApplicationMenuItem(BackendAdminMenuNames.Saas.GroupName, saasLocalizer["Menu:Saas"], icon: "fa fa-id-card-o");
            saasMenuItem.AddItem(new ApplicationMenuItem(BackendAdminMenuNames.Saas.Tenants, saasLocalizer["Menu:Tenants"], url: "/saas/tenants")
                .RequirePermissions(SaasPermissions.Tenants.Default));
            saasMenuItem.AddItem(new ApplicationMenuItem(BackendAdminMenuNames.Saas.Editions, saasLocalizer["Menu:Editions"], url: "/saas/Editions")
                .RequirePermissions(SaasPermissions.Editions.Default));

            context.Menu.GetAdministration().AddItem(saasMenuItem);

            return Task.CompletedTask;
        }
    }
}
