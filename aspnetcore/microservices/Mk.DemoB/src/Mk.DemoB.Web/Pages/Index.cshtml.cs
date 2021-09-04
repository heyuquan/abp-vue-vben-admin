using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace Mk.DemoB.Web.Pages
{
    public class IndexModel : DemoBPageModel
    {
        public void OnGet()
        {
            
        }

        public async Task OnPostLoginAsync()
        {
            await HttpContext.ChallengeAsync("oidc");
        }
    }
}