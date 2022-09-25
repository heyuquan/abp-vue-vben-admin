﻿using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace System
{
    /// <summary>
    /// 字符串方法扩展
    /// </summary>
    public static partial class StringExtentions
    {
        #region Format
        /// <summary>
        /// Formats a string to an invariant culture
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="objects">The objects.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string FormatInvariant(this string format, params object[] objects)
            => string.Format(CultureInfo.InvariantCulture, format, objects);

        /// <summary>
        /// Formats a string to the current culture.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="objects">The objects.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string FormatCurrent(this string format, params object[] objects)
            => string.Format(CultureInfo.CurrentCulture, format, objects);

        /// <summary>
        /// Formats a string to the current UI culture.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="objects">The objects.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string FormatCurrentUI(this string format, params object[] objects)
            => string.Format(CultureInfo.CurrentUICulture, format, objects);

        [DebuggerStepThrough]
        public static string FormatWith(this string format, IFormatProvider provider, params object[] args)
            => string.Format(provider, format, args);
        #endregion

        /// <summary>
        /// Determines whether this instance and given <paramref name="other"/> have the same value (ignoring case)
        /// </summary>
        /// <param name="value">The string to check equality.</param>
        /// <param name="other">The comparing with string.</param>
        /// <returns>
        /// <c>true</c> if the value of the comparing parameter is the same as this string; otherwise, <c>false</c>.
        /// </returns>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EqualsNoCase(this string value, string other)
        {
            return string.Compare(value, other, StringComparison.OrdinalIgnoreCase) == 0;
        }

        /// <summary>
        /// 删除字符串头部和尾部的回车/换行/空格
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>清除回车/换行/空格之后的字符串</returns>
        public static string TrimBlank(this string str)
        {
            if (str.IsNullOrEmpty2())
            {
                throw new NullReferenceException("字符串不可为空");
            }
            return str.TrimLeft().TrimRight();
        }

        /// <summary>
        /// 删除字符串尾部的回车/换行/空格
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>清除回车/换行/空格之后的字符串</returns>
        public static string TrimRight(this string str)
        {
            if (!str.IsNullOrEmpty2())
            {
                int i = 0;
                while ((i = str.Length) > 0)
                {
                    if (!str[i - 1].Equals(' ') && !str[i - 1].Equals('\r') && !str[i - 1].Equals('\n'))
                    {
                        break;
                    }
                    str = str.Substring(0, i - 1);
                }
            }
            return str;
        }

        /// <summary>
        /// 删除字符串头部的回车/换行/空格
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>清除回车/换行/空格之后的字符串</returns>
        public static string TrimLeft(this string str)
        {
            if (!str.IsNullOrEmpty2())
            {
                while (str.Length > 0)
                {
                    if (!str[0].Equals(' ') && !str[0].Equals('\r') && !str[0].Equals('\n'))
                    {
                        break;
                    }
                    str = str.Substring(1);
                }
            }
            return str;
        }

        /// <summary>
        /// 移除换行
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveLine(this string str)
        {
            if (str.IsNullOrEmpty2())
            {
                return str;
            }
            return str.Replace("\r", "").Replace("\n", "");
        }

        /// <summary>
        /// 相同字符串的数量
        /// </summary>
        /// <param name="source">字符串</param>
        /// <param name="pattern">相比较字符串</param>
        /// <returns></returns>
        public static int MatchesCount(this string source, string pattern)
        {
            var result = source.IsNullOrEmpty2() ? 0 : Regex.Matches(source, pattern).Count;
            return result;
        }

        /// <summary>
        /// 获取字符串长度，计算方式：中文计2位，英文计1位
        /// </summary>
        /// <param name="source">字符串</param>
        /// <returns></returns>
        public static int CharCodeLength(string source)
        {
            var n = 0;
            foreach (var c in source.ToCharArray())
            {
                if (c < 128)
                    n++;
                else
                    n += 2;
            }

            return n;
        }


        #region Substring扩展

        /// <summary>
        /// SubString方法扩展
        /// </summary>
        /// <param name="str">截取字符串</param>
        /// <param name="length">要截取的长度</param>
        /// <returns>string</returns>
        public static string Substring(this string str, int length)
        {
            if (string.IsNullOrEmpty(str) || str.Length <= length)
            {
                return str;
            }
            return str.Substring(0, length);
        }

        /// <summary>
        /// 截取 指定字符 之前的文本
        /// </summary>
        /// <param name="text">要截取的字符串</param>
        /// <param name="delimiter">指定字符</param>
        /// <returns>返回 指定字符 之前的文本</returns>
        public static string SubstringToFirst(this string text, char delimiter)
        {
            if (text == null)
            {
                return null;
            }
            var num = text.IndexOf(delimiter);
            if (num >= 0)
            {
                return text.Substring(0, num);
            }
            return text;
        }

        #endregion

        /// <summary>
        /// 反射获取属性值
        /// </summary>
        /// <typeparam name="T">匿名对象</typeparam>
        /// <param name="t">匿名对象集合</param>
        /// <param name="propertyname">属性名</param>
        /// <returns></returns>
        public static string GetPropertyValue<T>(this T t, string propertyname)
        {
            Type type = typeof(T);
            PropertyInfo property = type.GetProperty(propertyname);

            if (property == null) return string.Empty;

            object o = property.GetValue(t, null);

            if (o == null) return string.Empty;

            return o.ToString();
        }

        private const string DumpStr = "------------------------------------------------";
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DebugDump(this string value, bool appendMarks = false)
        {
            Debug.WriteLine(value);
            Debug.WriteLineIf(appendMarks, DumpStr);
        }

        /// <summary>
        /// 若文本是空，则返回默认值
        /// </summary>
        /// <param name="text"></param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>返回 指定字符 之前的文本</returns>
        public static string GetValue(this string text, string defaultValue)
        {
            if (text.IsNullOrWhiteSpace2())
                return defaultValue;
            else
                return text;
        }

        /// <summary>
        /// Gets a substring of a string from beginning of the string if it exceeds maximum length.
        /// It adds a "..." postfix to end of the string if it's truncated.
        /// Returning string can not be longer than maxLength.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="str"/> is null</exception>
        public static string TruncateWithPostfix2(this string str, int maxLength)
        {
            return TruncateWithPostfix2(str, maxLength, "...");
        }

        /// <summary>
        /// Gets a substring of a string from beginning of the string if it exceeds maximum length.
        /// It adds given <paramref name="postfix"/> to end of the string if it's truncated.
        /// Returning string can not be longer than maxLength.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="str"/> is null</exception>
        public static string TruncateWithPostfix2(this string str, int maxLength, string postfix)
        {
            if (str == null)
            {
                return null;
            }

            if (str == string.Empty || maxLength == 0)
            {
                return string.Empty;
            }

            if (str.Length <= maxLength)
            {
                return str;
            }

            if (maxLength <= postfix.Length)
            {
                return postfix.Substring(maxLength);
            }

            return str.Substring(maxLength - postfix.Length) + postfix;
        }

        /// <summary>
        /// 把字符串中间的字符用...代替
        /// </summary>
        /// <param name="str"></param>
        /// <param name="count">前后各显示多少字符</param>
        /// <returns></returns>
        public static string TruncateMiddle(this string str, int count)
        {
            if (str.IsNullOrWhiteSpace2() || count <= 0)
            {
                return str;
            }

            if (str.Length <= (count * 2 + 3))      // 两边各count个字符，中间 3个 点儿.
            {
                return str;
            }

            return $"{str.Substring(0, count)}...{str.Substring(str.Length - count)}";
        }
    }
}