using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;
using Leopard.Saas.Localization;

namespace Leopard.Saas.Permissions
{
    public class SaasPermissionDefinitionProvider : PermissionDefinitionProvider
	{
		public override void Define(IPermissionDefinitionContext context)
		{
			var saasHostGroup = context.AddGroup(SaasPermissions.GroupName, L("Permission:Saas"), MultiTenancySides.Host);
			var tenantManagement = saasHostGroup.AddPermission(SaasPermissions.Tenants.Default, L("Permission:TenantManagement"), MultiTenancySides.Host);
			tenantManagement.AddChild(SaasPermissions.Tenants.Create, L("Permission:Create"), MultiTenancySides.Host);
			tenantManagement.AddChild(SaasPermissions.Tenants.Update, L("Permission:Edit"), MultiTenancySides.Host);
			tenantManagement.AddChild(SaasPermissions.Tenants.Delete, L("Permission:Delete"), MultiTenancySides.Host);
			tenantManagement.AddChild(SaasPermissions.Tenants.ManageFeatures, L("Permission:ManageFeatures"), MultiTenancySides.Host);
			tenantManagement.AddChild(SaasPermissions.Tenants.ManageConnectionStrings, L("Permission:ManageConnectionStrings"), MultiTenancySides.Host);
			tenantManagement.AddChild(SaasPermissions.Tenants.ViewChangeHistory, L("Permission:ViewChangeHistory"), MultiTenancySides.Host);
			var editionManagement = saasHostGroup.AddPermission(SaasPermissions.Editions.Default, L("Permission:EditionManagement"), MultiTenancySides.Host);
			editionManagement.AddChild(SaasPermissions.Editions.Create, L("Permission:Create"), MultiTenancySides.Host);
			editionManagement.AddChild(SaasPermissions.Editions.Update, L("Permission:Edit"), MultiTenancySides.Host);
			editionManagement.AddChild(SaasPermissions.Editions.Delete, L("Permission:Delete"), MultiTenancySides.Host);
			editionManagement.AddChild(SaasPermissions.Editions.ManageFeatures, L("Permission:ManageFeatures"), MultiTenancySides.Host);
			editionManagement.AddChild(SaasPermissions.Editions.ViewChangeHistory, L("Permission:ViewChangeHistory"), MultiTenancySides.Host);
		}

		private static LocalizableString L(string name)
		{
			return LocalizableString.Create<SaasResource>(name);
		}
	}
}
