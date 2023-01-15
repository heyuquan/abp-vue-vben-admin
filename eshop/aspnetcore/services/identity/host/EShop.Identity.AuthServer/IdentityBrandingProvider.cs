using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace EShop.Identity;

[Dependency(ReplaceServices = true)]
public class IdentityBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "EShop.Identity.AuthServer";
}
