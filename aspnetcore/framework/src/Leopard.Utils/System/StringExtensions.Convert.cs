using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace System
{
    /// <summary>
    /// 字符串方法扩展
    /// </summary>
    public static partial class StringExtentions
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
        /// 换为首字母大写的字符串（abp>>Abp）
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <returns>首字母大写的字符串</returns>
        public static string ToUpperHead(this string str)
        {
            if (str.IsNullOrWhiteSpace2() || (str[0] >= 'A' && str[0] <= 'Z'))
            {
                return str;
            }
            if (str.Length == 1)
            {
                return str.ToUpper();
            }
            return string.Format("{0}{1}", str[0].ToString().ToUpper(), str.Substring(1).ToLower());
        }

        /// <summary>
        /// 将字符串进行Unicode编码，变成形如“\u7f16\u7801”的形式
        /// </summary>
        /// <param name="source">要进行编号的字符串</param>
        public static string ToUnicodeString(this string source)
        {
            Regex regex = new Regex(@"[^\u0000-\u00ff]");
            return regex.Replace(source, m => string.Format(@"\u{0:x4}", (short)m.Value[0]));
        }

        /// <summary>
        /// 将形如“\u7f16\u7801”的Unicode字符串解码
        /// </summary>
        public static string FromUnicodeString(this string source)
        {
            Regex regex = new Regex(@"\\u([0-9a-fA-F]{4})", RegexOptions.Compiled);
            return regex.Replace(source,
                m =>
                {
                    short s;
                    if (short.TryParse(m.Groups[1].Value, NumberStyles.HexNumber, CultureInfo.InstalledUICulture, out s))
                    {
                        return "" + (char)s;
                    }
                    return m.Value;
                });
        }


        /// <summary>
        /// url进行编码
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string ToUrlEncode(this string url)
        {
            if (url.IsNullOrEmpty2())
            {
                return url;
            }
            return System.Web.HttpUtility.UrlEncode(url);
        }

        /// <summary>
        /// url进行解码
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string ToUrlDecode(this string url)
        {
            if (url.IsNullOrEmpty2())
            {
                return url;
            }
            return System.Web.HttpUtility.UrlDecode(url);
        }


        /// <summary>
        /// 将字符串转换为<see cref="byte"/>[]数组，默认编码为<see cref="Encoding.UTF8"/>
        /// </summary>
        public static byte[] ToBytes(this string value, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            return encoding.GetBytes(value);
        }

        /// <summary>
        /// 字符串转换成bool类型
        /// </summary>
        /// <param name="source">字符串</param>
        /// <returns></returns>
        public static bool ToBoolean(this string source)
        {
            bool reValue;
            bool.TryParse(source, out reValue);
            return reValue;
        }

        /// <summary>
        /// 转化为Byte型
        /// </summary>
        /// <returns>Byte</returns>
        public static Byte ToByte(this string source)
        {
            Byte reValue;
            Byte.TryParse(source, out reValue);
            return reValue;
        }

        /// <summary>
        /// 转化为Short型
        /// </summary>
        /// <returns>Short</returns>
        public static short ToShort(this string source)
        {
            short reValue;
            short.TryParse(source, out reValue);
            return reValue;
        }

        /// <summary>
        /// 转化为Short型
        /// </summary>
        /// <returns>Short</returns>
        public static short ToInt16(this string source)
        {
            short reValue;
            short.TryParse(source, out reValue);
            return reValue;
        }

        /// <summary>
        /// 转化为int32型
        /// </summary>
        /// <returns>int32</returns>
        public static int ToInt32(this string source)
        {
            int reValue;
            Int32.TryParse(source, out reValue);
            return reValue;
        }

        /// <summary>
        /// 转化为int64型
        /// </summary>
        /// <returns>int64</returns>
        public static long ToInt64(this string source)
        {
            long reValue;
            Int64.TryParse(source, out reValue);
            return reValue;
        }

        /// <summary>
        /// 转化为Double型
        /// </summary>
        /// <returns>decimal</returns>
        public static Double ToDouble(this string source)
        {
            Double reValue;
            Double.TryParse(source, out reValue);
            return reValue;
        }
        /// <summary>
        /// 转化为decimal型
        /// </summary>
        /// <returns>decimal</returns>
        public static decimal ToDecimal(this string source)
        {
            decimal reValue;
            decimal.TryParse(source, out reValue);
            return reValue;
        }

        /// <summary>
        /// 转化为数字类型的日期
        /// </summary>
        /// <returns>DateTime</returns>
        public static decimal ToDateTimeDecimal(this string source)
        {
            DateTime reValue;
            return DateTime.TryParse(source, out reValue) ? reValue.ToString("yyyyMMddHHmmss").ToDecimal() : 0;
        }

        /// <summary>
        /// 将时间转换成数字
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static decimal ToDateTimeDecimal(this DateTime source)
        {
            return source.ToString("yyyyMMddHHmmss").ToDecimal();
        }

        /// <summary>
        /// 转换成TextArea保存的格式；（textarea中的格式保存显示的时候会失效）
        /// 即将换行符转为br标签，tab转为4个空格
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToTextArea(this string @source)
        {
            return source.IsNullOrEmpty2() ? source : source.Replace("\n\r", "<br/>").Replace("\r", "<br>").Replace("\t", "　　");
        }

        /// <summary>
        /// 字符串拼接成的数组转换成集合
        /// </summary>
        /// <param name="arrStr">要转换的字符串</param>
        /// <param name="splitchar">分离字符(默认,)</param>
        /// <returns></returns>
        public static List<int> ToIntList(this string arrStr, char splitchar = ',')
        {
            if (arrStr.IsNullOrEmpty2())
            {
                return new List<int>();
            }
            else
            {
                try
                {
                    return arrStr.Split(splitchar).Select(m => m.ToInt32()).ToList();
                }
                catch (Exception exception)
                {
                    throw new Exception(exception.Message);
                }
            }
        }

        /// <summary>
        /// 将字符串转换成int类型的数组
        /// </summary>
        /// <param name="arrStr">要转换的字符串</param>
        /// <param name="splitchar">分离字符(默认,)</param>
        /// <returns></returns>
        public static int[] ToIntArray(this string arrStr, char splitchar = ',')
        {
            if (arrStr.IsNullOrEmpty2())
            {
                return new int[0];
            }
            try
            {
                int[] array = arrStr.Split(splitchar).Select(m => m.ToInt32()).ToArray();
                return array;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

    }
}
