using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static partial class StringExtentions
    {
        private static string[] strs =
        {
            "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v","w", "x", "y", "z",
            "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V","W", "X", "Y", "Z"
        };

        /// <summary>
        /// 创建伪随机字符串 （由26个小写字母+26个大写字母构成）
        /// </summary>
        /// <param name="str"></param>
        /// <param name="strleg">长度</param>
        /// <returns></returns>
        public static string CreateNonce(this string str, long strleg = 15)
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

        private static string[] nums =
        {
            "0","1","2","3","4","5","6","7","8","9"
        };

        /// <summary>
        /// 创建伪随机数字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="numleg"></param>
        /// <returns></returns>
        public static string CreateNumberNonce(this string str, int numleg = 4)
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
