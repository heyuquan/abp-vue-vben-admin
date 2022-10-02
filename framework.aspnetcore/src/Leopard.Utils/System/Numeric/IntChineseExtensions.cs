using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace System
{
    /// <summary>
    /// 数值中文表示法类型
    /// </summary>
    public enum ChineseIntType
    {
        /// <summary>
        /// 简体中文数字
        /// </summary>
        CN = 0,
        /// <summary>
        /// 繁体中文数字
        /// </summary>
        HK = 1,

        /// <summary>
        /// 大写中文数字外带元角分货币单位
        /// </summary>
        CNAmount = 5,
        /// <summary>
        /// 大写中文数字外带圆毫仙货币单位
        /// </summary>
        HKAmount = 6
    }

    /// <summary>
    /// 
    /// </summary>
    public static class IntChineseExtensions
    {
        private const string ConvertToChinese_StringFormat = "#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A##";
        private const string ConvertToChinese_Regex = @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))";
        private const string ConvertToChinese_StringMappingHK = "負點空零一二三四五六七八九空空空空空空空  十百千萬億兆京垓秭穰";
        private const string ConvertToChinese_StringMappingCN = "负点空零一二三四五六七八九空空空空空空空  十百千万亿兆京垓秭穰";
        private const string ConvertToChinese_StringMappingForHKAmount = "負圓空零壹贰叁肆伍陆柒捌玖空空空空空空空仙毫拾佰仟萬億兆京垓秭穰";
        private const string ConvertToChinese_StringMappingForCNAmount = "負元空零壹贰叁肆伍陆柒捌玖空空空空空空空分角拾佰仟萬億兆京垓秭穰";
        /// <summary>
        /// 将阿拉伯数字转为中文数字或者金额表达
        /// </summary>
        /// <param name="value">输入值</param>
        /// <param name="chineseIntType"></param>
        /// <returns></returns>
        public static string ConvertToChinese(this double value, ChineseIntType chineseIntType = ChineseIntType.CN)
        {
            string s = value.ToString(ConvertToChinese_StringFormat);
            string d = Regex.Replace(s, ConvertToChinese_Regex, "${b}${z}");
            MatchEvaluator convert = null;
            switch (chineseIntType)
            {
                case ChineseIntType.CN: convert = delegate (Match m) { return ConvertToChinese_StringMappingCN[m.Value[0] - '-'].ToString(); }; break;
                case ChineseIntType.HK: convert = delegate (Match m) { return ConvertToChinese_StringMappingHK[m.Value[0] - '-'].ToString(); }; break;
                case ChineseIntType.HKAmount: convert = delegate (Match m) { return ConvertToChinese_StringMappingForHKAmount[m.Value[0] - '-'].ToString(); }; break;
                case ChineseIntType.CNAmount: convert = delegate (Match m) { return ConvertToChinese_StringMappingForCNAmount[m.Value[0] - '-'].ToString(); }; break;
            }
            return Regex.Replace(d, ".", convert).Replace(" ", "").TrimEnd(new char[] { '點', '点' });
        }

        /// <summary>
        /// 将阿拉伯数字转为中文数字或者金额表达
        /// </summary>
        /// <param name="value">输入值</param>
        /// <param name="chineseIntType"></param>
        /// <returns></returns>
        public static string ConvertToChinese(this long value, ChineseIntType chineseIntType = ChineseIntType.CN)
        {
            return Convert.ToDouble(value).ConvertToChinese(chineseIntType);
        }


        /// <summary>
        /// 将阿拉伯数字转为中文数字或者金额表达
        /// </summary>
        /// <param name="value">输入值</param>
        /// <param name="chineseIntType"></param>
        /// <returns></returns>
        public static string ConvertToChinese(this int value, ChineseIntType chineseIntType = ChineseIntType.CN)
        {
            return Convert.ToDouble(value).ConvertToChinese(chineseIntType);
        }

        /// <summary>
        /// 将阿拉伯数字转为中文数字或者金额表达
        /// </summary>
        /// <param name="value">输入值</param>
        /// <param name="chineseIntType"></param>
        /// <returns></returns>
        public static string ConvertToChinese(this decimal value, ChineseIntType chineseIntType = ChineseIntType.CN)
        {
            return Convert.ToDouble(value).ConvertToChinese(chineseIntType);
        }

        /// <summary>
        /// 将阿拉伯数字转为中文数字或者金额表达
        /// </summary>
        /// <param name="value">输入值</param>
        /// <param name="chineseIntType"></param>
        /// <returns></returns>
        public static string ConvertToChinese(this float value, ChineseIntType chineseIntType = ChineseIntType.CN)
        {
            return Convert.ToDouble(value).ConvertToChinese(chineseIntType);
        }

        /// <summary>
        /// 将阿拉伯数字转换为中文数字
        /// </summary>
        /// <param name="value"></param>
        /// <param name="chineseIntType"></param>
        /// <returns></returns>
        public static string ToZhTwNumber(this uint value, ChineseIntType chineseIntType = ChineseIntType.CN)
        {
            return Convert.ToDouble(value).ConvertToChinese(chineseIntType);
        }

    }
}
