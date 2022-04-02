using System.Xml.Serialization;

namespace Leopard.Serialization.Xml
{
    public class BaseXmlPayload : IXmlPayload
    {
        public XmlSerializerNamespaces Xmlns { get; set; } = new XmlSerializerNamespaces();
    }
}
