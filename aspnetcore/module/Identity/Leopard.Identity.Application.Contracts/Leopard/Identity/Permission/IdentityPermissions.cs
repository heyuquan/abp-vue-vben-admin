using Volo.Abp.Reflection;

namespace Leopard.Identity
{
	public static class IdentityPermissions
	{
		public static string[] GetAll()
		{
			return ReflectionHelper.GetPublicConstantsRecursively(typeof(IdentityPermissions));
		}

		public const string GroupName = "AbpIdentity";

		public const string SettingManagement = "AbpIdentity.SettingManagement";

		public static class Roles
		{
			public const string Default = "AbpIdentity.Roles";

			public const string Create = "AbpIdentity.Roles.Create";

			public const string Update = "AbpIdentity.Roles.Update";

			public const string Delete = "AbpIdentity.Roles.Delete";

			public const string ManagePermissions = "AbpIdentity.Roles.ManagePermissions";

			public const string ViewChangeHistory = "AuditLogging.ViewChangeHistory:Volo.Abp.Identity.IdentityRole";
		}

		public static class Users
		{
			public const string Default = "AbpIdentity.Users";

			public const string Create = "AbpIdentity.Users.Create";

			public const string Update = "AbpIdentity.Users.Update";

			public const string Delete = "AbpIdentity.Users.Delete";

			public const string ManagePermissions = "AbpIdentity.Users.ManagePermissions";

			public const string ViewChangeHistory = "AuditLogging.ViewChangeHistory:Volo.Abp.Identity.IdentityUser";
		}

		public static class ClaimTypes
		{
			public const string Default = "AbpIdentity.ClaimTypes";

			public const string Create = "AbpIdentity.ClaimTypes.Create";

			public const string Update = "AbpIdentity.ClaimTypes.Update";

			public const string Delete = "AbpIdentity.ClaimTypes.Delete";
		}

		public static class UserLookup
		{
			public const string Default = "AbpIdentity.UserLookup";
		}

		public static class OrganizationUnits
		{
			public const string Default = "AbpIdentity.OrganizationUnits";

			public const string ManageOU = "AbpIdentity.OrganizationUnits.ManageOU";

			public const string ManageRoles = "AbpIdentity.OrganizationUnits.ManageRoles";

			public const string ManageUsers = "AbpIdentity.OrganizationUnits.ManageMembers";
		}

		public class SecurityLog
		{
			public const string Default = GroupName + ".SecurityLog";
			public const string Delete = Default + ".Delete";
		}
	}
}
