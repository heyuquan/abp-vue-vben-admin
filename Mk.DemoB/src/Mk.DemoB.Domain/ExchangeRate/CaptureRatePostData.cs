using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Mk.DemoB.ExchangeRate
{
    public class CaptureRatePostData
    {
        /// <summary>
        /// eg：美元
        /// </summary>
        [JsonPropertyName("pjname")]
        public string pjname{ get; set; }
    }
}
