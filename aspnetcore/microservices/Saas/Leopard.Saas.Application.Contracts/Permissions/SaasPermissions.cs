using Volo.Abp.Reflection;

namespace Leopard.Saas.Permissions
{
    public class SaasPermissions
    {
        public const string GroupName = "Saas";

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(SaasPermissions));
        }
    }
}