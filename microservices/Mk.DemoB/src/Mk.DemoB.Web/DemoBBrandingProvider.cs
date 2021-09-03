using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace Mk.DemoB.Web
{
    [Dependency(ReplaceServices = true)]
    public class DemoBBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "DemoB";
    }
}
