using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Timing;

namespace Mk.DemoB.Dto.Timing
{
    public class MultiTimeTypeResult
    {
        [DisableDateTimeNormalization]
        public DateTime LocalDateTime { get; set; }

        public DateTime UtcDateTime { get; set; }

        /// <summary>
        /// 未处理的时间
        /// </summary>
        [DisableDateTimeNormalization]
        public DateTime UnHandleDateTime { get; set; }
    }
}
