namespace Leopard.Serialization.Xml
{
    using System;
    using System.IO;
    using System.Text;

    // XmlSerializer参考： https://www.cnblogs.com/linyefeilyft/p/3379063.html
    // 另外：xml文档的操作类封装   https://www.cnblogs.com/Can-daydayup/p/16058817.html

    // xmlns:前缀="命名空间"
    // XML命名空间详解
    // https://blog.csdn.net/zisgood/article/details/98942674
    // xml序列化帮助类不考虑命名空间，太复杂了。只做简单的封装。遇到命名空间场景再考虑

    public class XmlSerializer : ISerializer
    {
        /// <summary>
        /// Converts to xml string and returns
        /// </summary>
        /// <returns></returns>
        public string Serialize<TPayload>(TPayload item)
        {
            string result = string.Empty;
            Encoding encode = Encoding.UTF8;

            using (MemoryStream output = new MemoryStream())
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(TPayload));
                serializer.Serialize(output, item);
                result = encode.GetString(output.ToArray());
            }

            return result;
        }

        public TPayload Deserialize<TPayload>(string content) where TPayload : class
        {
            TPayload result = null;
            Encoding encode = Encoding.UTF8;
            
            using (MemoryStream input = new MemoryStream(encode.GetBytes(content)))
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(TPayload));
                result = serializer.Deserialize(input) as TPayload;
            }

            return result;
        }

    }
}
