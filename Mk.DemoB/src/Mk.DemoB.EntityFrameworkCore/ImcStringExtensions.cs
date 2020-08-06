using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace System
{
    public static class ImcStringExtensions
    {
        /// <summary>
        /// 功能：驼峰命名转下划线命名（AbpUser>>abp_user）
        /// 小写和大写紧挨一起的地方,加上分隔符,然后全部转小写
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static String CamelToUnder(this String c)
        {
            String separator = "_";
            c = Regex.Replace(c, "([a-z])([A-Z])", "$1" + separator + "$2").ToLower();
            return c;
        }


        /// <summary>
        /// 功能：下划线命名转大驼峰命名（abp_user>>AbpUser）
        /// 将下划线替换为空格,将字符串根据空格分割成数组,再将每个单词首字母大写
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static String UnderToCamel(this String s)
        {
            String separator = "_";
            String under = "";
            s = s.ToLower().Replace(separator, " ");
            string[] sarr = s.Split(new char[] { ' ' });
            for (int i = 0; i < sarr.Length; i++)
            {
                String w = sarr[i].Substring(0, 1).ToUpper() + sarr[i].Substring(1);
                under += w;
            }
            return under;
        }
    }
}