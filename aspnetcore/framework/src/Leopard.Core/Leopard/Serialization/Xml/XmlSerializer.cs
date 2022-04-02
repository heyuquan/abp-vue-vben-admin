namespace Leopard.Serialization.Xml
{
    using System;
    using System.IO;
    using System.Text;

    // 改进 https://www.cnblogs.com/linyefeilyft/p/3379063.html
    // Encoding
    // Namespaces
    // 验证：item.GetType().IsBaseOn(typeof(IXmlPayload))
    // xml文档的操作类封装   https://www.cnblogs.com/Can-daydayup/p/16058817.html

    public class XmlSerializer : ISerializer
    {
        public sealed class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding => Encoding.UTF8;
        }

        public TPayload Deserialize<TPayload>(string content)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(TPayload));
            using (var reader = new StringReader(content))
            {
                return (TPayload)serializer.Deserialize(reader);
            }
        }

        /// <summary>
        /// Converts to xml string and returns
        /// </summary>
        /// <returns></returns>
        public string Serialize<TPayload>(TPayload item)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(TPayload));
            var stringWriter = new Utf8StringWriter();
            if (item.GetType().IsBaseOn(typeof(IXmlPayload)))   // 待验证  
            {
                if (((IXmlPayload)item).Xmlns.Count > 0)
                    serializer.Serialize(stringWriter, item, ((IXmlPayload)item).Xmlns);
                else
                    serializer.Serialize(stringWriter, item);
            }
            else
                serializer.Serialize(stringWriter, item);

            return stringWriter.ToString();
        }
    }
}
