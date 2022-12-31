using Volo.Abp.Localization;
using Volo.Abp.TextTemplating;
using Leopard.Account.Localization;

namespace Leopard.Account.Public.Emailing.Templates
{
	public class AccountEmailTemplateDefinitionProvider : TemplateDefinitionProvider
	{
		public override void Define(ITemplateDefinitionContext context)
		{
			var array = new TemplateDefinition[1];
			int num = 0;
			string name = "Abp.Account.PasswordResetLink";
			var displayName = LocalizableString.Create<LeopardAccountResource>("TextTemplate:Abp.Account.PasswordResetLink");
			array[num] = new TemplateDefinition(name, typeof(LeopardAccountResource), displayName, false, "Abp.StandardEmailTemplates.Layout", null).WithVirtualFilePath("/Volo/Abp/Account/Emailing/Templates/PasswordResetLink.tpl", true);
			context.Add(array);
			var array2 = new TemplateDefinition[1];
			int num2 = 0;
			string name2 = "Abp.Account.EmailConfirmationLink";
			displayName = LocalizableString.Create<LeopardAccountResource>("TextTemplate:Abp.Account.EmailConfirmationLink");
			array2[num2] = new TemplateDefinition(name2, typeof(LeopardAccountResource), displayName, false, "Abp.StandardEmailTemplates.Layout", null).WithVirtualFilePath("/Volo/Abp/Account/Emailing/Templates/EmailConfirmationLink.tpl", true);
			context.Add(array2);
		}
	}
}
