﻿using Volo.Abp.Settings;

namespace SSO.AuthServer.Settings
{
    public class AuthServerSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            //Define your own settings here. Example:
            //context.Add(new SettingDefinition(AuthServerSettings.MySetting1));
        }
    }
}
