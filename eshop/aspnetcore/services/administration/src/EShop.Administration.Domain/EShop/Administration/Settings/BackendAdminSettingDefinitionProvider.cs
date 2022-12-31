using Volo.Abp.Settings;

namespace EShop.Administration.Settings
{
    public class AdministrationSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            //Define your own settings here. Example:
            //context.Add(new SettingDefinition(AdministrationSettings.MySetting1));
        }
    }
}
