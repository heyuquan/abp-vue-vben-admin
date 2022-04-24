using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace System
{
    /// <summary>
    /// 字符串方法扩展
    /// </summary>
    public static partial class StringExtentions
    {
        // IsNullOrEmpty和IsNullOrWhiteSpace的区别
        // https://www.cnblogs.com/liushen/p/6756849.html

        /// <summary>
        /// 是否为空
        /// abp 中定义了同样功效的方法，会冲突，所以命名此处为2
        /// </summary>
        /// <param name="source">字符串</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty2(this string source)
        {
            return string.IsNullOrEmpty(source);
        }

        /// <summary>
        /// 指示指定的字符串是 null、string.empty、""、只有空格、换行符的字符串
        /// abp 中定义了同样功效的方法，会冲突，所以命名此处为2
        /// </summary>
        /// <param name="source">要测试的字符串</param>
        /// <returns>如果 value 参数为 null、string.empty、""、只有空格、换行符的字符串，则为 true。</returns>
        public static bool IsNullOrWhiteSpace2(this string source)
        {
            return string.IsNullOrWhiteSpace(source);
        }

        /// <summary>
        /// 是否有值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasValue(this string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Determines whether the string is all white space. Empty string will return false.
        /// </summary>
        /// <param name="value">The string to test whether it is all white space.</param>
        /// <returns>
        /// 	<c>true</c> if the string is all white space; otherwise, <c>false</c>.
        /// </returns>
        [DebuggerStepThrough]
        public static bool IsWhiteSpace(this string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (value.Length == 0)
                return false;

            for (int i = 0; i < value.Length; i++)
            {
                if (!char.IsWhiteSpace(value[i]))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 是否匹配相等
        /// </summary>
        /// <param name="source">字符串</param>
        /// <param name="pattern">相比较字符串</param>
        /// <returns></returns>
        public static bool IsMatch(this string source, string pattern)
        {
            return !source.IsNullOrEmpty() && Regex.IsMatch(source, pattern);
        }

        /// <summary>
        /// 相同的字符串
        /// </summary>
        /// <param name="source">字符串</param>
        /// <param name="pattern">相比较字符串</param>
        /// <returns>相同的字符串</returns>
        public static string Match(this string source, string pattern)
        {
            string result = source.IsNullOrEmpty() ? "" : Regex.Match(source, pattern).Value;
            return result;
        }

        /// <summary>
        /// 是否是url地址
        /// </summary>
        /// <param name="checkStr">字符串</param>
        /// <returns></returns>
        public static bool IsUrlAddress(this string checkStr)
        {
            return !checkStr.IsNullOrEmpty() && Regex.IsMatch(checkStr, @"[a-zA-z]+://[^s]*", RegexOptions.Compiled);
        }

        /// <summary>
        /// 判断是否是正确的电子邮件格式
        /// </summary>
        /// <param name="source">字符串</param>
        /// <returns>bool</returns>
        public static bool IsEmail(this string source)
        {
            return Regex.IsMatch(source, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", RegexOptions.Compiled);
            //return Regex.IsMatch(source, @"^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$", RegexOptions.Compiled);
        }

        #region 验证身份证是否有效
        // https://www.cnblogs.com/ChineseMoonGod/p/5193606.html
        /// 身份证号验证
        /// eg:
        /// 正确的：522324197508045617
        /// 错误的：612427199901281214

        /// <summary>
        /// 验证身份证是否有效
        /// </summary>
        /// <param name="source">输入字符</param>
        /// <returns>返回一个bool类型的值</returns>
        public static bool IsIDCard(this string source)
        {
            switch (source.Length)
            {
                case 18:
                    {
                        return source.IsIDCard18();
                    }
                case 15:
                    {
                        return source.IsIDCard15();
                    }
                default:
                    return false;
            }
        }

        /// <summary>
        /// 验证输入字符串为18位的身份证号码
        /// </summary>
        /// <param name="source">输入字符</param>
        /// <returns>返回一个bool类型的值</returns>
        public static bool IsIDCard18(this string source)
        {
            long n = 0;
            if (long.TryParse(source.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(source.Replace('x', '0').Replace('X', '0'), out n) == false)
            {
                return false;//数字验证
            }
            const string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(source.Remove(2), StringComparison.Ordinal) == -1)
            {
                return false;//省份验证
            }
            string birth = source.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time;
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证
            }
            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] ai = source.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(wi[i]) * int.Parse(ai[i].ToString());
            }
            int y = -1;
            Math.DivRem(sum, 11, out y);
            if (arrVarifyCode[y] != source.Substring(17, 1).ToLower())
            {
                return false;//校验码验证  
            }
            return true;//符合GB11643-1999标准  
        }
        /// <summary>
        /// 验证输入字符串为15位的身份证号码
        /// </summary>
        /// <param name="source">输入字符</param>
        /// <returns>返回一个bool类型的值</returns>
        public static bool IsIDCard15(this string source)
        {
            long n = 0;
            if (long.TryParse(source, out n) == false || n < Math.Pow(10, 14))
            {
                return false;//数字验证
            }
            const string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(source.Remove(2), StringComparison.Ordinal) == -1)
            {
                return false;//省份验证
            }
            string birth = source.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            DateTime time;
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证  
            }
            return true;
        }
        #endregion

        #region 是否由数字、26个英文字母或者下划线組成的字串
        /// <summary>
        /// 是否由数字、26个英文字母或者下划线組成的字串 
        /// </summary>
        /// <param name="str">输入字符</param>
        /// <returns></returns>
        public static bool IsNzx(this string str)
        {
            return Regex.Match(str, "^[0-9a-zA-Z_]+$").Success;
        }
        #endregion

        #region 由数字、26个英文字母、汉字組成的字串
        /// <summary>
        /// 由数字、26个英文字母、汉字組成的字串
        /// </summary>
        /// <param name="str">输入字符</param>
        /// <returns></returns>
        public static bool IsSzzmChinese(this string str)
        {
            return Regex.Match(str, @"^[0-9a-zA-Z\u4e00-\u9fa5]+$").Success;
        }
        #endregion

        #region 由数字、26个英文字母組成的字串
        /// <summary>
        /// 是否由数字、26个英文字母組成的字串
        /// </summary>
        /// <param name="str">输入字符</param>
        /// <returns></returns>
        public static bool IsSzzm(this string str)
        {
            return Regex.Match(str, @"^[0-9a-zA-Z]+$").Success;
        }
        #endregion

        /// <summary>
        /// 判断是否是正确的邮政编码格式
        /// </summary>
        /// <param name="source">字符串</param>
        /// <returns></returns>
        public static bool IsPostcode(this string source)
        {
            return Regex.IsMatch(source, @"^[1-9]{1}(\d){5}$", RegexOptions.Compiled);
        }

        /// <summary>
        /// 验证是否是有效传真号码
        /// </summary>
        /// <param name="source">输入字符</param>
        /// <returns>返回一个bool类型的值</returns>
        public static bool IsFax(this string source)
        {
            return Regex.IsMatch(source, @"^[+]{0,1}(\d){1,3}[ ]?([-]?((\d)|[ ]){1,12})+$", RegexOptions.Compiled);
        }

        /// <summary>
        /// 验证手机号是否合法 号段为13,14,15,16,17,18,19  0，86开头将自动识别
        /// </summary>
        /// <param name="source">字符串</param>
        /// <returns></returns>
        public static bool IsMobilePhone(this string source)
        {
            if (!source.StartsWith("1"))
            {
                source = source.TrimStart(new char[] { '8', '6', }).TrimStart('0');
            }
            return Regex.IsMatch(source, @"^(13|14|15|16|17|18|19)\d{9}$");

            //return Regex.IsMatch(source, @"^(86)*0*13\d{9}$", RegexOptions.Compiled);
        }

        /// <summary>
        /// 判断是否是正确的中国固定电话 。格式010-85849685
        /// </summary>
        /// <param name="source">字符串</param>
        /// <returns></returns>
        public static bool IsTelephone(this string source)
        {
            return Regex.IsMatch(source, @"^(0[0-9]{2,3}\-)?([2-9][0-9]{6,7})+(\-[0-9]{1,4})?$", RegexOptions.IgnoreCase);
            // return Regex.IsMatch(source, @"^((\d{3,4})|\d{3,4}-|\s)?\d{8}$", RegexOptions.Compiled);
        }

        /// <summary>
        /// 验证输入字符串为电话号码
        /// </summary>
        /// <param name="str">输入字符</param>
        /// <returns>返回一个bool类型的值</returns>
        public static bool IsPhone(this string str)
        {
            return Regex.IsMatch(str, @"(^(\d{2,4}[-_－—]?)?\d{3,8}([-_－—]?\d{3,8})?([-_－—]?\d{1,7})?$)|(^0?1[35]\d{9}$)");
            //弱一点的验证：  @"\d{3,4}-\d{7,8}"         
        }

        /// <summary>
        /// 包含html标签
        /// </summary>
        /// <param name="source">字符串</param>
        /// <returns></returns>
        public static bool IsHasHtml(this string source)
        {
            Regex reg = new Regex(@"<|>");
            return reg.IsMatch(source);
        }

        /// <summary>
        /// 是否匹配正则表达式，匹配返回true，否则false
        /// </summary>
        /// <param name="source">字符串</param>
        /// <param name="regex">正则表达式</param>
        /// <returns></returns>
        public static bool IsMatchRegex(this string source, string regex)
        {
            Regex r = new Regex(regex);
            return r.IsMatch(source);
        }

        /// <summary>
        /// 判断字符串是否是IP，如果是返回True，不是返回False
        /// </summary>
        /// <param name="source">字符串</param>
        /// <returns></returns>
        public static bool IsIp(this string source)
        {
            Regex regex = new Regex(@"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])"
                + @"\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$", RegexOptions.Compiled);
            return regex.Match(source).Success;
        }

        /// <summary>
        /// 是否包含中文或全角字符
        /// </summary>
        /// <param name="checkStr">字符串</param>
        /// <returns></returns>
        public static bool IsHasChinese(this string checkStr)
        {
            ASCIIEncoding n = new ASCIIEncoding();
            byte[] b = n.GetBytes(checkStr);
            for (int i = 0; i <= b.Length - 1; i++)
                if (b[i] == 63) return true;  //判断是否(T)为汉字或全脚符号 
            return false;
        }

        /// <summary>
        /// 是否是中文
        /// </summary>
        /// <param name="checkStr">字符串</param>
        /// <returns></returns>
        public static bool IsAllChinese(this string checkStr)
        {
            checkStr = checkStr.Trim();
            if (checkStr == string.Empty) return false;
            Regex reg = new Regex(@"^([\u4e00-\u9fa5]*)$", RegexOptions.Compiled);
            return reg.IsMatch(checkStr);
        }

        /// <summary>
        /// 是否为正整数
        /// </summary>
        /// <param name="intStr">字符串</param>
        /// <returns></returns>
        public static bool IsInt(this string intStr)
        {
            Regex regex = new Regex("^\\d+$", RegexOptions.Compiled);
            return regex.IsMatch(intStr.Trim());
        }

        /// <summary>
        /// 非负整数
        /// </summary>
        /// <param name="intStr">字符串</param>
        /// <returns></returns>
        public static bool IsIntWithZero(this string intStr)
        {
            return Regex.IsMatch(intStr, @"^\\d+$", RegexOptions.Compiled);
        }

        /// <summary>
        /// 是否是数字
        /// </summary>
        /// <param name="checkStr">字符串</param>
        /// <returns></returns>
        public static bool IsNumber(this string checkStr)
        {
            return Regex.IsMatch(checkStr, @"^[+-]?[0123456789]*[.]?[0123456789]*$", RegexOptions.Compiled);
        }

        /// <summary>
        /// 是否是Decimal类型数据
        /// </summary>
        /// <param name="checkStr">字符串</param>
        /// <returns></returns>
        public static bool IsDecimal(this string checkStr)
        {
            return Regex.IsMatch(checkStr, @"^[0-9]+/.?[0-9]{0,2}$", RegexOptions.Compiled);
        }

        /// <summary>
        /// 是否是DateTime类型数据
        /// </summary>
        /// <param name="checkStr">字符串</param>
        /// <returns></returns>
        public static bool IsDateTime(this string checkStr)
        {
            return Regex.IsMatch(checkStr, @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-)) ((20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d)$");
        }

        /// <summary>
        /// 是否为日期型字符串
        /// </summary>
        /// <param name="checkStr">日期字符串(2005-6-30)</param>
        /// <returns></returns>
        public static bool IsDate(this string checkStr)
        {
            return Regex.IsMatch(checkStr, @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$");
        }



        /// <summary>
        /// 是否为时间型字符串
        /// </summary>
        /// <param name="checkStr">时间字符串(15:00:00)</param>
        /// <returns></returns>
        public static bool IsTime(this string checkStr)
        {
            return Regex.IsMatch(checkStr, @"^((20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d)$");
        }

        /// <summary>
        /// 检查一个 string 是否为有效的 HTTP url 格式
        /// str格式：(eg：www.baidu.com|http://www.baidu.com|https://www.baidu.com)
        /// </summary>
        /// <param name="str"></param>
        /// <param name="resultURI"></param>
        /// <returns></returns>
        public static bool IsValidHttpURL(this string str, out Uri resultURI)
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
        /// </summary>
        /// <param name="str"></param>
        /// <param name="resultURI"></param>
        /// <returns></returns>
        public static bool IsValidFileURL(this string str, out Uri resultURI)
        {
            if (Uri.TryCreate(str, UriKind.Absolute, out resultURI))
                return (resultURI.Scheme == Uri.UriSchemeFile);

            return false;
        }

        /// <summary>
        /// 检查对象的输入长度
        /// </summary>
        /// <param name="str">输入字符</param>
        /// <param name="length">指定的长度</param>
        /// <returns>false 太长，true -太短</returns>
        public static bool CheckLength(this string str, int length)
        {
            if (str.Length > length)
            {
                return false;//长度太长
            }
            return str.Length >= length;
        }
    }
}
