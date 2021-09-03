using System;
using System.Collections.Generic;
using System.Text;

namespace Mk.DemoB.Dto.ExchangeRates
{
    /// <summary>
    /// 汇率批次数据
    /// </summary>
    public class ExchangeRateBatchDto
    {
        public ExchangeRateBatchDto()
        {
            ExchangeRates = new HashSet<ExchangeRateDto>();
        }

        /// <summary>
        /// 本次抓取批号
        /// </summary>
        public string CaptureBatchNumber { get; set; }
        /// <summary>
        /// 抓取时间
        /// </summary>
        public DateTime CaptureTime { get; set; }

        public ICollection<ExchangeRateDto> ExchangeRates { get; set; }
    }
}
