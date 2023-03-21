﻿namespace EShop.Common.Shared
{

    // #、应用标识
    // #、api地址会根据ModuleName生成  eg: /api/identity-auth/***

    public class ModuleNames
    {
        //public const string AuthServer = "EShopAuthServer";
        public const string IdentityAuthServer = "IdentityAuth";
        //public const string Saas = "LeopardSaas";
        public const string Identity = "Identity";
        public const string Account = "Account";
        public const string AccountAdmin = "AccountAdmin";
        public const string AccountPublic = "AccountPublic";
        public const string Administration = "Admin";

        public const string AdministrationGateway = "AdminGateway";
        public const string InternalGateway = "InternalGateway";
        public const string PublicWebGateway = "PublicWebGateway";
    }
}
