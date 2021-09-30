using Volo.Abp.Reflection;

namespace Leopard.Saas.Permissions
{
    public class SaasPermissions
	{
		public static string[] GetAll()
		{
			return ReflectionHelper.GetPublicConstantsRecursively(typeof(SaasPermissions));
		}

		public const string GroupName = "Saas";

		public static class Tenants
		{
			public const string Default = "Saas.Tenants";

			public const string Create = "Saas.Tenants.Create";

			public const string Update = "Saas.Tenants.Update";

			public const string Delete = "Saas.Tenants.Delete";

			public const string ManageFeatures = "Saas.Tenants.ManageFeatures";

			public const string ManageConnectionStrings = "Saas.Tenants.ManageConnectionStrings";

			public const string ViewChangeHistory = "AuditLogging.ViewChangeHistory:Volo.Saas.Tenant";
		}

		public static class Editions
		{
			public const string Default = "Saas.Editions";

			public const string Create = "Saas.Editions.Create";

			public const string Update = "Saas.Editions.Update";

			public const string Delete = "Saas.Editions.Delete";

			public const string ManageFeatures = "Saas.Editions.ManageFeatures";

			public const string ViewChangeHistory = "AuditLogging.ViewChangeHistory:Volo.Saas.Edition";
		}
	}
}
