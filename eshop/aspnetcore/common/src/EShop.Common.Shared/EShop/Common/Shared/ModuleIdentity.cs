using Leopard;

namespace EShop.Common.Shared
{
    public static class ModuleIdentity
    {
        public static class Auth
        {
            public const string Name = ModuleNames.AuthServer;
            public const ApplicationServiceType ServiceType = ApplicationServiceType.AuthHost;
        }

        public static class AuthIdentityServer
        {
            public const string Name = ModuleNames.AuthServerIdentityServer;
            public const ApplicationServiceType ServiceType = ApplicationServiceType.AuthIdentityServer;
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
