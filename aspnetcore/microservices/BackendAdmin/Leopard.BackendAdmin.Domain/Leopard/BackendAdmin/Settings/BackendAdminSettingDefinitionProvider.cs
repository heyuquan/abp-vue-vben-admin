﻿using Volo.Abp.Settings;

namespace Leopard.BackendAdmin.Settings
{
    public class BackendAdminSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            //Define your own settings here. Example:
            //context.Add(new SettingDefinition(BackendAdminSettings.MySetting1));
        }
    }
}
