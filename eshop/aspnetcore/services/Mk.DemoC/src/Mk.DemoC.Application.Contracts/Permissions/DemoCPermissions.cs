using Volo.Abp.Reflection;

namespace Mk.DemoC.Permissions
{
    public class DemoCPermissions
    {
        public const string GroupName = "DemoC";

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(DemoCPermissions));
        }
    }
}