using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Mk.DemoB.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using Volo.Abp.Localization;

namespace Mk.DemoB.Application
{
    [Route("api/demob/lanaguage")]
    public class LanguageAppService : DemoBAppService
    {
        private readonly IStringLocalizer<DemoBResource> _localizer;

        public LanguageAppService(IStringLocalizer<DemoBResource> localizer)
        {
            _localizer = localizer;
        }

        public string StringLocal()
        {
            return _localizer["TextParams", "hyq", "beek"];
        }

        public string StringLocal_ZH()
        {
            string text = "";
            using (CultureHelper.Use(CultureInfo.GetCultureInfo("zh-Hans")))
            {
                text = _localizer["TextParams", "hyq", "beek"].ToString();
            }

            return text;
        }

        public string StringLocal_EN()
        {
            string text = "";
            using (CultureHelper.Use(CultureInfo.GetCultureInfo("en")))
            {
                text = _localizer["TextParams", "hyq", "beek"].ToString();
            }

            return text;
        }
    }
}
