using EShop.Administration.Localization;
using Leopard.Identity;
using Leopard.UI.Navigation;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Identity.Localization;
using Volo.Abp.UI.Navigation;

namespace EShop.Administration.Web.Navigation
{
    public class AdministrationWebMenuContributor : IMenuContributor
    {
        public virtual Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name != LeopardStandardMenus.Main)
            {
                return Task.CompletedTask;
            }

            var AdministrationLocalizer = context.GetLocalizer<AdministrationResource>();

            #region ==== vben demo begin =====
            var dashboardMenuItem = new ApplicationMenuItem("Demo.Dashboard", "Dashboard", url: "/dashboard", icon: "ion:grid-outline");
            dashboardMenuItem.WithCustomData("IsGroup", true);
            dashboardMenuItem.WithCustomData("Component", "LAYOUT");
            dashboardMenuItem.WithCustomData("Redirect", "/dashboard/analysis");
            dashboardMenuItem.AddItem(
                    new ApplicationMenuItem("Demo.Dashboard.Analysis", "分析页", url: "/dashboard/analysis")
                        .WithCustomData("Component", "/dashboard/analysis/index")
                );
            dashboardMenuItem.AddItem(
                    new ApplicationMenuItem("Demo.Dashboard.Workbench", "工作台", url: "/dashboard/workbench")
                        .WithCustomData("Component", "/dashboard/workbench/index")
                );

            context.Menu.GetAdministration().AddItem(dashboardMenuItem);

            var aboutMenuItem = new ApplicationMenuItem("Demo.About", "关于", order: 99999, url: "/about", icon: "simple-icons:about-dot-me");
            dashboardMenuItem.WithCustomData("IsGroup", true);
            dashboardMenuItem.WithCustomData("Component", "LAYOUT");
            dashboardMenuItem.WithCustomData("Redirect", "/about/index");
            dashboardMenuItem.WithCustomData("HideChildrenInMenu", true);
            aboutMenuItem.AddItem(
                    new ApplicationMenuItem("Demo.About.AboutPage", "关于", url: "/about/index")
                        .WithCustomData("Component", "/sys/about/index")
                        .WithCustomData("HideMenu", true)
                );

            context.Menu.GetAdministration().AddItem(aboutMenuItem);
            #endregion ==== vben demo end =====           

            //#region ==== saas begin =====
            //var saasLocalizer = context.GetLocalizer<SaasResource>();
            //var saasMenuItem = new ApplicationMenuItem(AdministrationMenuNames.Saas.GroupName, saasLocalizer["Menu:Saas"]
            //    , url: "/saas", icon: "ion:git-branch-outline", customData: new { IsGroup = true, Component = "LAYOUT", Redirect = "/saas/tenants" });
            //saasMenuItem.AddItem(new ApplicationMenuItem(AdministrationMenuNames.Saas.Tenants, saasLocalizer["Menu:Tenants"]
            //    , url: "/saas/tenants", customData: new { Component = "/saas/tenant/index" })
            //    .RequirePermissions(SaasPermissions.Tenants.Default));
            ////saasMenuItem.AddItem(new ApplicationMenuItem(AdministrationMenuNames.Saas.Editions, saasLocalizer["Menu:Editions"]
            ////    , url: "/saas/editions", customData: new { Component = "/saas/edition/index" })
            ////    .RequirePermissions(SaasPermissions.Editions.Default));

            //context.Menu.GetAdministration().AddItem(saasMenuItem);
            //#endregion ==== saas end =====


            #region ==== identity begin ===== 
            var identityLocalizer = context.GetLocalizer<IdentityResource>();

            var manageMenuItem = new ApplicationMenuItem(AdministrationMenuNames.Administration.GroupName
                , AdministrationLocalizer["Menu:Administration"], icon: "ion:cube-outline", url: "/manage");
            manageMenuItem.WithCustomData("IsGroup", true);
            manageMenuItem.WithCustomData("Component", "LAYOUT");
            manageMenuItem.WithCustomData("Redirect", "/identity/users");

            var identityMenuItem = new ApplicationMenuItem(AdministrationMenuNames.Administration.Identity.GroupName
                , identityLocalizer["Menu:IdentityManagement"], url: "/identity");
            identityMenuItem.WithCustomData("IsGroup", true);
            identityMenuItem.WithCustomData("Component", AdministrationMenuNames.Administration.Identity.GroupName);
            identityMenuItem.WithCustomData("Redirect", "/identity/users");
            identityMenuItem.AddItem(
                    new ApplicationMenuItem(AdministrationMenuNames.Administration.Identity.OrganizationUnits
                        , identityLocalizer["Menu:OrganizationUnits"], url: "/identity/organization-units")
                    .RequirePermissions(IdentityPermissions.OrganizationUnits.Default)
                    .WithCustomData("Component", "/identity/organization-units/index")
                );

            identityMenuItem.AddItem(
                    new ApplicationMenuItem(AdministrationMenuNames.Administration.Identity.Roles, identityLocalizer["Menu:Roles"], url: "/identity/roles")
                    .RequirePermissions(IdentityPermissions.Roles.Default)
                    .WithCustomData("Component", "/identity/role/index")
                );

            identityMenuItem.AddItem(
                new ApplicationMenuItem(AdministrationMenuNames.Administration.Identity.Users, identityLocalizer["Menu:Users"], url: "/identity/users")
                .RequirePermissions(IdentityPermissions.Users.Default)
                .WithCustomData("Component", "/identity/user/index")
                );
            //identityMenuItem.AddItem(new ApplicationMenuItem(AdministrationMenuNames.Administration.Identity.ClaimTypes, identityLocalizer["Menu:ClaimTypes"]
            //    , url: "/identity/claim-types", customData: new { Component = "/identity/claim-types/index" })
            //    .RequirePermissions(IdentityPermissions.ClaimTypes.Default));
            //identityMenuItem.AddItem(new ApplicationMenuItem(AdministrationMenuNames.Administration.Identity.SecurityLog, identityLocalizer["Menu:SecurityLog"]
            //    , url: "/identity/security-log", customData: new { Component = "/identity/security-log/index" })
            //    .RequirePermissions(IdentityPermissions.SecurityLog.Default));

            manageMenuItem.AddItem(identityMenuItem);
            context.Menu.GetAdministration().AddItem(manageMenuItem);
            #endregion ==== identity end =====


            // settings
            //var settingMenuItem = new ApplicationMenuItem(AdministrationMenuNames.Settings.GroupName
            //    , AdministrationLocalizer["Menu:Settings"], icon: "fa fa-id-card-o", url: "/setting-management");
            //context.Menu.GetAdministration().AddItem(settingMenuItem).RequirePermissions(AdministrationPermissions.Settings.Default);

            return Task.CompletedTask;
        }
    }
}
