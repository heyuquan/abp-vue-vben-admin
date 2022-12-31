using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Newtonsoft.Json
{
    // JObject      用于操作JSON对象
    // JArray       用语操作JSON数组
    // JValue       表示数组中的值
    // JProperty    表示对象中的属性, 以"key/value"形式
    // JToken       用于存放Linq to JSON查询后的结果

    // 在JObject层次结构中按名称搜索特定的JToken
    // https://codingdict.com/questions/26221
    //     JObject jo = JObject.Parse(json);
    //     foreach (JToken token in jo.FindTokens("text"))
    //     {
    //          Console.WriteLine(token.Path + ": " + token.ToString());
    //     }

    /// <summary>
    /// Newtonsoft 扩展
    /// </summary>
    public static class JsonExtensions
    {
        /// <summary>
        /// 在JSON中查找具有给定名称的令牌的 所有 出现
        /// （查找单个出现使用：jObject.SelectToken("routes[0].legs[0].distance.text").Value&lt;T>()）
        /// </summary>
        /// <param name="containerToken"></param>
        /// <param name="name">字段名称</param>
        /// <returns></returns>
        public static List<JToken> FindTokens(this JToken containerToken, string name)
        {
            List<JToken> matches = new List<JToken>();
            FindTokens(containerToken, name, matches);
            return matches;
        }

        private static void FindTokens(JToken containerToken, string name, List<JToken> matches)
        {
            if (containerToken.Type == JTokenType.Object)
            {
                foreach (JProperty child in containerToken.Children<JProperty>())
                {
                    if (child.Name == name)
                    {
                        matches.Add(child.Value);
                    }
                    FindTokens(child.Value, name, matches);
                }
            }
            else if (containerToken.Type == JTokenType.Array)
            {
                foreach (JToken child in containerToken.Children())
                {
                    FindTokens(child, name, matches);
                }
            }
        }
    }
}
