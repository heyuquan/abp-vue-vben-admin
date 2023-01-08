using Volo.Abp.Settings;

namespace EShop.Identity.Settings;

public class IdentitySettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(IdentitySettings.MySetting1));
    }
}
