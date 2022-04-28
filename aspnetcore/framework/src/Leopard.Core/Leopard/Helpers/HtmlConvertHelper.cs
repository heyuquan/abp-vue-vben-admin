﻿using Ganss.XSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Leopard.Helpers
{
    /// <summary>
    /// html 处理类
    /// </summary>
    public static class HtmlConvertHelper
    {
        //初始化HtmlSanitizer
        static HtmlSanitizer htmlSanitizer = new HtmlSanitizer();

        /// <summary>
        /// 去掉html中可能存在的xss攻击脚本
        /// </summary>
        /// <param name="html">html代码</param>
        /// <returns>过滤后的代码</returns>
        public static string SafeHtml(string html)
        {
            if (!string.IsNullOrWhiteSpace(html) && htmlSanitizer != null)
                html = htmlSanitizer.Sanitize(html);

            return html;
        }

        /// <summary>
        /// 移除html文本中的标签
        /// </summary>
        /// <param name="Htmlstring">HTML文本值</param>
        /// <returns></returns>
        public static string TextNoHTML(string Htmlstring)
        {
            //删除脚本
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([/r/n])[/s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "/", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "/xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "/xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "/xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "/xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(/d+);", "", RegexOptions.IgnoreCase);
            //替换掉 < 和 > 标记
            Htmlstring = Htmlstring.Replace("<", "");
            Htmlstring = Htmlstring.Replace(">", "");
            Htmlstring = Htmlstring.Replace("\r\n", "");
            Htmlstring = Htmlstring.Replace("\r", "");
            Htmlstring = Htmlstring.Replace("\n", "");
            //返回去掉html标记的字符串
            return Htmlstring;
        }

        /// <summary>  
        /// 获取Img的路径  
        /// </summary>  
        /// <param name="htmlText">Html字符串文本</param>  
        /// <returns>以数组形式返回图片路径</returns>  
        public static string[] GetHtmlImageUrlList(string htmlText)
        {
            Regex regImg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);
            //新建一个matches的MatchCollection对象 保存 匹配对象个数(img标签)  
            MatchCollection matches = regImg.Matches(htmlText);
            int i = 0;
            string[] sUrlList = new string[matches.Count];
            //遍历所有的img标签对象  
            foreach (Match match in matches)
            {
                //获取所有Img的路径src,并保存到数组中  
                sUrlList[i++] = match.Groups["imgUrl"].Value;
            }
            return sUrlList;
        }

        /// <summary>
        /// Text转为Html显示
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string TextToHtml(string text)
        {
            return $"<pre>{HttpUtility.HtmlEncode(text)}</pre>";
        }

        /// <summary>
        /// html编码
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string HtmlDecode(string text)
        {
            return HttpUtility.HtmlDecode(text);
        }

        /// <summary>
        /// html解码
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string HtmlEncode(string text)
        {
            return HttpUtility.HtmlEncode(text);
        }

    }
}