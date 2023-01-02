﻿using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace EShop.AuthServer
{
    [Dependency(ReplaceServices = true)]
    public class AuthServerBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "AuthServer";
    }
}
