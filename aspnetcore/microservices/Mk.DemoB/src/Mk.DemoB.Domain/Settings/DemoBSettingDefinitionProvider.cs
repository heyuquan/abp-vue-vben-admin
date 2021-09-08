using Volo.Abp.Settings;

namespace Mk.DemoB.Settings
{
    public class DemoBSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            //Define your own settings here. Example:
            //context.Add(new SettingDefinition(DemoBSettings.MySetting1));

            context.Add(new SettingDefinition(DemoBSettings.CompanyName, "BAT"));
            context.Add(new SettingDefinition(name: DemoBSettings.CompanySecretKey, defaultValue: "123456", isEncrypted: true));

        }
    }
}
