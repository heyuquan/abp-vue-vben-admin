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

        public static class IdentityAuth
        {
            public const string Name = ModuleNames.IdentityAuthServer;
            public const ApplicationServiceType ServiceType = ApplicationServiceType.IdentityAuthServer;
        }

        public static class Administration
        {
            public const string Name = ModuleNames.Administration;
            public const ApplicationServiceType ServiceType = ApplicationServiceType.ApiHost;
        }

        public static class AdministrationGateway
        {
            public const string Name = ModuleNames.AdministrationGateway;
            public const ApplicationServiceType ServiceType = ApplicationServiceType.GateWay;
        }

        public static class InternalGateway
        {
            public const string Name = ModuleNames.InternalGateway;
            public const ApplicationServiceType ServiceType = ApplicationServiceType.GateWay;
        }

        public static class PublicWebGateway
        {
            public const string Name = ModuleNames.PublicWebGateway;
            public const ApplicationServiceType ServiceType = ApplicationServiceType.GateWay;
        }
    }
}
