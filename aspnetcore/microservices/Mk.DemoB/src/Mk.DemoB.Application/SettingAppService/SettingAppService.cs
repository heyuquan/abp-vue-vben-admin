using Microsoft.AspNetCore.Mvc;
using Mk.DemoB.Settings;
using System.Threading.Tasks;
using Volo.Abp.Settings;

namespace Mk.DemoB.SettingAppService
{
    [Route("api/demob/setting")]
    public class SettingAppService: DemoBAppService
    {
        private readonly ISettingProvider _settingProvider;

        public SettingAppService(ISettingProvider settingProvider)
        {
            _settingProvider = settingProvider;
        }

        public async Task<string> GetAsync()
        {
            string companyName = await _settingProvider.GetOrNullAsync(DemoBSettings.CompanyName);

            string companySecretKey = await _settingProvider.GetOrNullAsync(DemoBSettings.CompanySecretKey);

            bool enableSsl = await _settingProvider.GetAsync<bool>("Abp.Mailing.Smtp.EnableSsl");

            int port = (await _settingProvider.GetAsync<int>("Abp.Mailing.Smtp.Port"));

            return await Task.FromResult($"{companyName}_{enableSsl}_{port}_{companySecretKey}");
        }

    }
}
