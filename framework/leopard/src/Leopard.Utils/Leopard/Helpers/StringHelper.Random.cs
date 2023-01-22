using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leopard.Helpers
{
    public static class StringHelper
    {
        // private static readonly char[] CA = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/".ToCharArray();
        private static char[] strs = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray();

        /// <summary>
        /// 创建伪随机字符串 （由26个小写字母+26个大写字母构成）
        /// </summary>
        /// <param name="strleg">长度</param>
        /// <returns></returns>
        public static string CreateRandomChar(long strleg = 15)
        {
            Random r = new Random();
            StringBuilder sb = new StringBuilder();
            var length = strs.Length;
            for (int i = 0; i < strleg; i++)
            {
                sb.Append(strs[r.Next(length - 1)]);
            }
            return sb.ToString();
        }

        private static char[] nums = "0123456789".ToCharArray();

        /// <summary>
        /// 创建伪随机数字符串
        /// </summary>
        /// <param name="numleg"></param>
        /// <returns></returns>
        public static string CreateRandomNumber(int numleg = 4)
        {
            Random r = new Random();
            StringBuilder sb = new StringBuilder();
            var length = nums.Length;
            for (int i = 0; i < numleg; i++)
            {
                sb.Append(nums[r.Next(length - 1)]);
            }
            return sb.ToString();
        }
    }
}
