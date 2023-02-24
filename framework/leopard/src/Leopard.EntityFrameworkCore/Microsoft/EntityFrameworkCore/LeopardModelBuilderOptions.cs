using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.EntityFrameworkCore
{
    public class LeopardModelBuilderOptions
    {
        /// <summary>
        /// 将大驼峰命名转为mysql规范的小写加下划线  Eg：AbpUsers >> abp_users
        /// </summary>
        public bool EnableCameNamelToUnder { get; set; } = false;

        public DefaultDecimalPrecisionOptions DefaultDecimalPrecisionOptions { get; set; } = new();
    }

    public class DefaultDecimalPrecisionOptions
    {
        public bool IsEnable { get; set; } = true;
        public int Precision { get; set; } = 18;
        public int Scale { get; set; } = 4;

    }
}
