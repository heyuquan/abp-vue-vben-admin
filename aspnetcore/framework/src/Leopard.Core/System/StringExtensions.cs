using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace System
{
    public static class StringExtensions
    {
        /// <summary>
        /// 功能：驼峰命名转下划线命名（AbpUser>>abp_user）
        /// 小写和大写紧挨一起的地方,加上分隔符,然后全部转小写
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static String CamelToUnder(this String str)
        {
            String separator = "_";
            str = Regex.Replace(str, "([a-z])([A-Z])", "$1" + separator + "$2").ToLower();
            return str;
        }


        /// <summary>
        /// 功能：下划线命名转大驼峰命名（abp_user>>AbpUser）
        /// 将下划线替换为空格,将字符串根据空格分割成数组,再将每个单词首字母大写
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static String UnderToCamel(this String str)
        {
            String separator = "_";
            String under = "";
            str = str.ToLower().Replace(separator, " ");
            string[] sarr = str.Split(new char[] { ' ' });
            for (int i = 0; i < sarr.Length; i++)
            {
                String w = sarr[i].Substring(0, 1).ToUpper() + sarr[i].Substring(1);
                under += w;
            }
            return under;
        }

        /// <summary>
        /// 检查一个 string 是否为有效的 HTTP url 格式
        /// str格式：(eg：www.baidu.com|http://www.baidu.com|https://www.baidu.com)
        /// </summary>
        /// <param name="resultURI"></param>
        /// <returns></returns>
        public static bool ValidHttpURL(this string str, out Uri resultURI)
        {
            if (str.StartsWith("www."))
            {
                str = "http://" + str;
            }
            if (Uri.TryCreate(str, UriKind.Absolute, out resultURI))
                return (resultURI.Scheme == Uri.UriSchemeHttp ||
                        resultURI.Scheme == Uri.UriSchemeHttps);

            return false;
        }

        /// <summary>
        /// 检查一个 string 是否为有效的 File url 格式
        /// str格式：file://<host>/<path>
        /// </summary>
        /// <param name="resultURI"></param>
        /// <returns></returns>
        public static bool ValidFileURL(this string str, out Uri resultURI)
        {
            if (Uri.TryCreate(str, UriKind.Absolute, out resultURI))
                return (resultURI.Scheme == Uri.UriSchemeFile);

            return false;
        }

    }
}
