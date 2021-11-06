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

            #region ==== vben demo begin =====
            var dashboardMenuItem = new ApplicationMenuItem("Demo.Dashboard", "Dashboard"
                , url: "/dashboard", icon: "ion:grid-outline", customData: new { Component = "LAYOUT", Redirect = "/dashboard/analysis" });
            dashboardMenuItem.AddItem(new ApplicationMenuItem("Demo.Dashboard.Analysis", "分析页"
                , url: "/dashboard/analysis", customData: new { Component = "/dashboard/analysis/index" }));
            dashboardMenuItem.AddItem(new ApplicationMenuItem("Demo.Dashboard.Workbench", "工作台"
                , url: "/dashboard/workbench", customData: new { Component = "/dashboard/workbench/index" }));

            context.Menu.GetAdministration().AddItem(dashboardMenuItem);

            var aboutMenuItem = new ApplicationMenuItem("Demo.About", "关于", order: 99999
                , url: "/about", icon: "simple-icons:about-dot-me"
                , customData: new { Component = "LAYOUT", Redirect = "/about/index", HideChildrenInMenu = true });
            aboutMenuItem.AddItem(new ApplicationMenuItem("Demo.About.AboutPage", "关于"
                , url: "/about/index", customData: new { Component = "/sys/about/index", HideMenu = true }));

            context.Menu.GetAdministration().AddItem(aboutMenuItem);
            #endregion ==== vben demo end =====           

            #region ==== saas begin =====
            var saasLocalizer = context.GetLocalizer<SaasResource>();
            var saasMenuItem = new ApplicationMenuItem(BackendAdminMenuNames.Saas.GroupName, saasLocalizer["Menu:Saas"]
                , url: "/saas", icon: "ion:git-branch-outline", customData: new { Component = "LAYOUT", Redirect = "/saas/tenants" });
            saasMenuItem.AddItem(new ApplicationMenuItem(BackendAdminMenuNames.Saas.Tenants, saasLocalizer["Menu:Tenants"]
                , url: "/saas/tenants", customData: new { Component = "/saas/tenant/index" })
                .RequirePermissions(SaasPermissions.Tenants.Default));
            //saasMenuItem.AddItem(new ApplicationMenuItem(BackendAdminMenuNames.Saas.Editions, saasLocalizer["Menu:Editions"]
            //    , url: "/saas/editions", customData: new { Component = "/saas/edition/index" })
            //    .RequirePermissions(SaasPermissions.Editions.Default));

            context.Menu.GetAdministration().AddItem(saasMenuItem);
            #endregion ==== saas end =====


            #region ==== identity begin ===== 
            var identityLocalizer = context.GetLocalizer<IdentityResource>();

            var manageMenuItem = new ApplicationMenuItem(BackendAdminMenuNames.BackendAdmin.GroupName
                , backendAdminLocalizer["Menu:Administration"], icon: "ion:cube-outline", url: "/manage"
                , customData: new { Component = "LAYOUT", Redirect = "/identity/users" });

            var identityMenuItem = new ApplicationMenuItem(BackendAdminMenuNames.BackendAdmin.Identity.GroupName
                , identityLocalizer["Menu:IdentityManagement"], url: "/identity"
                , customData: new { Component = BackendAdminMenuNames.BackendAdmin.Identity.GroupName, Redirect = "/identity/users" });
            identityMenuItem.AddItem(new ApplicationMenuItem(BackendAdminMenuNames.BackendAdmin.Identity.OrganizationUnits, identityLocalizer["Menu:OrganizationUnits"]
                , url: "/identity/organization-units", customData: new { Component = "/identity/organization-units/index" })
                .RequirePermissions(IdentityPermissions.OrganizationUnits.Default));
            identityMenuItem.AddItem(new ApplicationMenuItem(BackendAdminMenuNames.BackendAdmin.Identity.Roles, identityLocalizer["Menu:Roles"]
                , url: "/identity/roles", customData: new { Component = "/identity/roles/index" })
                .RequirePermissions(IdentityPermissions.Roles.Default));
            identityMenuItem.AddItem(new ApplicationMenuItem(BackendAdminMenuNames.BackendAdmin.Identity.Users, identityLocalizer["Menu:Users"]
                , url: "/identity/users", customData: new { Component = "/identity/users/index" })
                .RequirePermissions(IdentityPermissions.Users.Default));
            identityMenuItem.AddItem(new ApplicationMenuItem(BackendAdminMenuNames.BackendAdmin.Identity.ClaimTypes, identityLocalizer["Menu:ClaimTypes"]
                , url: "/identity/claim-types", customData: new { Component = "/identity/claim-types/index" })
                .RequirePermissions(IdentityPermissions.ClaimTypes.Default));
            identityMenuItem.AddItem(new ApplicationMenuItem(BackendAdminMenuNames.BackendAdmin.Identity.SecurityLog, identityLocalizer["Menu:SecurityLog"]
                , url: "/identity/security-log", customData: new { Component = "/identity/security-log/index" })
                .RequirePermissions(IdentityPermissions.SecurityLog.Default));

            manageMenuItem.AddItem(identityMenuItem);
            context.Menu.GetAdministration().AddItem(manageMenuItem);
            #endregion ==== identity end =====


            // settings
            //var settingMenuItem = new ApplicationMenuItem(BackendAdminMenuNames.Settings.GroupName
            //    , backendAdminLocalizer["Menu:Settings"], icon: "fa fa-id-card-o", url: "/setting-management");
            //context.Menu.GetAdministration().AddItem(settingMenuItem).RequirePermissions(BackendAdminPermissions.Settings.Default);

            return Task.CompletedTask;
        }
    }
}
