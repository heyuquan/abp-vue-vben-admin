using Leopard.Crypto;
using Leopard.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Leopard.Http
{
    /// <summary>
    /// 请求校验结果。
    /// </summary>
    public class CheckResult
    {
        public bool Success { get; set; }

        public string Body { get; set; }
    }

    /// <summary>
    /// 服务提供方工具类。
    /// </summary>
    public class DefaultRequestSignCheck
    {
        private const string HEADER_SIGN_LIST = "header-sign-list";

        /// <summary>
        /// 校验请求签名，不支持带上传文件的HTTP请求。
        /// </summary>
        /// <param name="request">HttpRequest对象实例</param>
        /// <param name="secret">APP密钥</param>
        /// <returns>校验结果</returns>
        public static CheckResult CheckSign(HttpRequest request, string secret)
        {
            CheckResult result = new CheckResult();
            string ctype = request.ContentType;
            if (ctype.StartsWith(Constants.CTYPE_APP_JSON) || ctype.StartsWith(Constants.CTYPE_TEXT_XML) || ctype.StartsWith(Constants.CTYPE_TEXT_PLAIN) || ctype.StartsWith(Constants.CTYPE_APPLICATION_XML))
            {
                result.Body = GetStreamAsString(request, GetRequestCharset(ctype));
                result.Success = CheckSignInternal(request, result.Body, secret);
            }
            else if (ctype.StartsWith(Constants.CTYPE_FORM_DATA))
            {
                result.Success = CheckSignInternal(request, null, secret);
            }
            else
            {
                throw new Exception("Unspported request");
            }
            return result;
        }

        /// <summary>
        /// 校验请求签名，适用于Content-Type为application/x-www-form-urlencoded或multipart/form-data的GET或POST请求。
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <param name="secret">app对应的secret</param>
        /// <returns>true：校验通过；false：校验不通过</returns>
        public static bool CheckSign4FormRequest(HttpRequest request, string secret)
        {
            return CheckSignInternal(request, null, secret);
        }

        /// <summary>
        /// 校验请求签名，适用于Content-Type为text/xml或text/json的POST请求。
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <param name="body">请求体的文本内容</param>
        /// <param name="secret">app对应的secret</param>
        /// <returns>true：校验通过；false：校验不通过</returns>
        public static bool CheckSign4TextRequest(HttpRequest request, string body, string secret)
        {
            return CheckSignInternal(request, body, secret);
        }

        /// <summary>
        /// 签名规则：hex(md5(secret+sorted(header_params+url_params+form_params)+body)+secret)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="body"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        private static bool CheckSignInternal(HttpRequest request, string body, string secret)
        {
            string charset = GetRequestCharset(request.ContentType);
            Dictionary<string, string> queryMap = GetQueryMap(request, charset);

            string remoteSign = null;
            if (queryMap.ContainsKey(Constants.SIGN))
            {
                remoteSign = queryMap[Constants.SIGN];
            }

            if (remoteSign.IsNullOrWhiteSpace2())
                return false;
            else
            {
                string localSign = GenerateSign(request, body, secret);
                return localSign.Equals(remoteSign);
            }
        }

        private static void AddAll(IDictionary<string, string> dest, IDictionary<string, string> from)
        {
            if (from != null && from.Count > 0)
            {
                IEnumerator<KeyValuePair<string, string>> em = from.GetEnumerator();
                while (em.MoveNext())
                {
                    KeyValuePair<string, string> kvp = em.Current;
                    dest.Add(kvp.Key, kvp.Value);
                }
            }
        }

        /// <summary>
        /// 生成签名
        /// 签名规则：hex(md5(secret+sorted(header_params+url_params+form_params)+body)+secret)
        /// 十六进制（简写为hex或下标16）
        /// </summary>
        public static string GenerateSign(HttpRequest request, string body, string secret)
        {
            SortedDictionary<string, string> parameters = new SortedDictionary<string, string>(StringComparer.Ordinal);
            string charset = GetRequestCharset(request.ContentType);

            // 1. 获取header参数
            AddAll(parameters, GetHeaderMap(request, charset));

            // 2. 获取url参数
            Dictionary<string, string> queryMap = GetQueryMap(request, charset);
            AddAll(parameters, queryMap);

            // 3. 获取form参数
            AddAll(parameters, GetFormMap(request));

            return GenerateSign(parameters, body, secret, charset);
        }

        /// <summary>
        /// 生成签名
        /// 签名规则：hex(md5(secret+sorted(parameters)+body)+secret)
        /// 十六进制（简写为hex或下标16）
        /// </summary>
        public static string GenerateSign(SortedDictionary<string, string> parameters, string body, string secret, string charset)
        {
            IEnumerator<KeyValuePair<string, string>> em = parameters.GetEnumerator();

            // 第1步：把所有参数名和参数值串在一起
            StringBuilder query = new StringBuilder(secret);
            while (em.MoveNext())
            {
                string key = em.Current.Key;
                if (!Constants.SIGN.Equals(key))
                {
                    string value = em.Current.Value;
                    query.Append(key).Append(value);
                }
            }
            if (body != null)
            {
                query.Append(body);
            }

            query.Append(secret);

            // 第2步：使用MD5加密
            return CryptoGuide.Md5.Encrypt32(query.ToString(), Encoding.GetEncoding(charset));
        }

        private static string GetRequestCharset(string ctype)
        {
            string charset = "utf-8";
            if (!string.IsNullOrEmpty(ctype))
            {
                string[] entires = ctype.Split(';');
                foreach (string entry in entires)
                {
                    string _entry = entry.Trim();
                    if (_entry.StartsWith("charset"))
                    {
                        string[] pair = _entry.Split('=');
                        if (pair.Length == 2)
                        {
                            if (!string.IsNullOrEmpty(pair[1]))
                            {
                                charset = pair[1].Trim();
                            }
                        }
                        break;
                    }
                }
            }
            return charset;
        }

        public static Dictionary<string, string> GetHeaderMap(HttpRequest request, string charset)
        {
            Dictionary<string, string> headerMap = new Dictionary<string, string>();
            string signList = request.Headers[HEADER_SIGN_LIST];    // Header_SIGN_LIST 里面标注的 项 才进行签名
            if (!string.IsNullOrEmpty(signList))
            {
                string[] keys = signList.Split(',');
                foreach (string key in keys)
                {
                    string value = request.Headers[key];
                    if (string.IsNullOrEmpty(value))
                    {
                        headerMap.Add(key, "");
                    }
                    else
                    {
                        headerMap.Add(key, UrlHelper.UrlDecode(value, Encoding.GetEncoding(charset)));
                    }
                }
            }
            return headerMap;
        }

        public static Dictionary<string, string> GetQueryMap(HttpRequest request, string charset)
        {
            Dictionary<string, string> queryMap = new Dictionary<string, string>();
            string queryString = request.QueryString.ToString();
            if (!string.IsNullOrEmpty(queryString))
            {
                // queryString = queryString.Substring(1); // 忽略?号
                string[] parameters = queryString.Split('&');
                foreach (string parameter in parameters)
                {
                    string[] kv = parameter.Split('=');
                    if (kv.Length == 2)
                    {
                        string key = UrlHelper.UrlDecode(kv[0], Encoding.GetEncoding(charset));
                        string value = UrlHelper.UrlDecode(kv[1], Encoding.GetEncoding(charset));
                        queryMap.Add(key, value);
                    }
                    else if (kv.Length == 1)
                    {
                        string key = UrlHelper.UrlDecode(kv[0], Encoding.GetEncoding(charset));
                        queryMap.Add(key, "");
                    }
                }
            }
            return queryMap;
        }

        public static Dictionary<string, string> GetFormMap(HttpRequest request)
        {
            Dictionary<string, string> formMap = new Dictionary<string, string>();
            IFormCollection form = request.Form;
            ICollection<string> keys = form.Keys;
            foreach (string key in keys)
            {
                string value = request.Form[key];
                if (string.IsNullOrEmpty(value))
                {
                    formMap.Add(key, "");
                }
                else
                {
                    formMap.Add(key, value);
                }
            }
            return formMap;
        }

        public static string GetStreamAsString(HttpRequest request, string charset)
        {
            Stream stream = null;
            StreamReader reader = null;

            try
            {
                // 以字符流的方式读取HTTP请求体
                stream = request.Body;
                reader = new StreamReader(stream, Encoding.GetEncoding(charset));
                return reader.ReadToEnd();
            }
            finally
            {
                // 释放资源
                if (reader != null) reader.Close();
                if (stream != null) stream.Close();
            }
        }

        /// <summary>
        /// 检查请求到达服务器端是否已经超过指定的分钟数，如果超过则拒绝请求。
        /// </summary>
        /// <returns>true代表不超过，false代表超过。</returns>
        public static bool CheckTimestamp(HttpRequest request, int minutes)
        {
            string ts = request.Query[Constants.TIMESTAMP];
            if (!string.IsNullOrEmpty(ts))
            {
                DateTime remote = DateTime.ParseExact(ts, Constants.DATE_TIME_FORMAT, null);
                DateTime local = DateTime.Now;
                return remote.AddMinutes(minutes).CompareTo(local) > 0;
            }
            else
            {
                return false;
            }
        }
    }
}
