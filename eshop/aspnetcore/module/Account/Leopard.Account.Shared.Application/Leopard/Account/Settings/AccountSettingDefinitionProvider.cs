using Volo.Abp.Localization;
using Volo.Abp.Settings;
using Leopard.Account.Localization;

namespace Leopard.Account.Settings
{
    public class AccountSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            context.Add(new SettingDefinition[]
            {
                new SettingDefinition(AccountSettingNames.IsSelfRegistrationEnabled, "true", L("DisplayName:IsSelfRegistrationEnabled"), L("Description:IsSelfRegistrationEnabled"), true),
                new SettingDefinition(AccountSettingNames.EnableLocalLogin, "true", L("DisplayName:EnableLocalLogin"), L("Description:EnableLocalLogin"), true),
                new SettingDefinition(AccountSettingNames.TwoFactorLogin.IsRememberBrowserEnabled, "true", L("DisplayName:IsRememberBrowserEnabled"), null, true)
            });
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<LeopardAccountResource>(name);
        }
    }
}