using Ganss.XSS;
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
        /// <param name="htmlText">HTML文本值</param>
        /// <returns></returns>
        public static string TextNoHTML(string htmlText)
        {
            //删除脚本
            htmlText = Regex.Replace(htmlText, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML
            htmlText = Regex.Replace(htmlText, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            htmlText = Regex.Replace(htmlText, @"([/r/n])[/s]+", "", RegexOptions.IgnoreCase);
            htmlText = Regex.Replace(htmlText, @"-->", "", RegexOptions.IgnoreCase);
            htmlText = Regex.Replace(htmlText, @"<!--.*", "", RegexOptions.IgnoreCase);
            htmlText = Regex.Replace(htmlText, @"&(quot|#34);", "/", RegexOptions.IgnoreCase);
            htmlText = Regex.Replace(htmlText, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            htmlText = Regex.Replace(htmlText, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            htmlText = Regex.Replace(htmlText, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            htmlText = Regex.Replace(htmlText, @"&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
            htmlText = Regex.Replace(htmlText, @"&(iexcl|#161);", "/xa1", RegexOptions.IgnoreCase);
            htmlText = Regex.Replace(htmlText, @"&(cent|#162);", "/xa2", RegexOptions.IgnoreCase);
            htmlText = Regex.Replace(htmlText, @"&(pound|#163);", "/xa3", RegexOptions.IgnoreCase);
            htmlText = Regex.Replace(htmlText, @"&(copy|#169);", "/xa9", RegexOptions.IgnoreCase);
            htmlText = Regex.Replace(htmlText, @"&#(/d+);", "", RegexOptions.IgnoreCase);
            //替换掉 < 和 > 标记
            htmlText = htmlText.Replace("<", "");
            htmlText = htmlText.Replace(">", "");
            htmlText = htmlText.Replace("\r\n", "");
            htmlText = htmlText.Replace("\r", "");
            htmlText = htmlText.Replace("\n", "");
            //返回去掉html标记的字符串
            return htmlText;
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

        // 把富文本的回车转为br标签
        // https://www.ddpool.cn/article/12913.html
        // 方式一：string.replace(/(rn|n|r)/gm, "<br/>")
        // 方式二：用 <pre></pre>标签，<pre> 标签的一个常见应用就是用来表示计算机的源代码。可以识别字符串中的‘/n’，‘/r/n’, 制表符，空格...
        // 方式三：用<textarea></textarea>展示，这样那边编辑的什么，这边就会显示什么

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
