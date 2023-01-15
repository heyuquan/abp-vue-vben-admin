using Leopard;

namespace EShop.Common.Shared
{
    // 改为从配置文件里面读取，就不用这个类了
    public static class ModuleIdentity
    {
        public static class Identity
        {
            public const string Name = ModuleNames.Identity;
            public const ApplicationServiceType ServiceType = ApplicationServiceType.ApiHost;
        }

        public static class IdentityAuthServer
        {
            public const string Name = ModuleNames.IdentityAuthServer;
            public const ApplicationServiceType ServiceType = ApplicationServiceType.IdentityAuthServer;
        }

        public static class Administration
        {
            public const string Name = ModuleNames.Administration;
            public const ApplicationServiceType ServiceType = ApplicationServiceType.ApiHost;
        }

        public static class AdministrationAppGateway
        {
            public const string Name = "AdministrationAppGateway";
            public const ApplicationServiceType ServiceType = ApplicationServiceType.GateWay;
        }

        public static class InternalGateway
        {
            public const string Name = "InternalGateway";
            public const ApplicationServiceType ServiceType = ApplicationServiceType.GateWay;
        }

        public static class PublicWebSiteGateway
        {
            public const string Name = "PublicWebSiteGateway";
            public const ApplicationServiceType ServiceType = ApplicationServiceType.GateWay;
        }
    }
}
