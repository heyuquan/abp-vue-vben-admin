using Mk.DemoB.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Mk.DemoB.Web.Pages
{
    public abstract class DemoBPageModel : AbpPageModel
    {
        protected DemoBPageModel()
        {
            LocalizationResourceType = typeof(DemoBResource);
        }
    }
}