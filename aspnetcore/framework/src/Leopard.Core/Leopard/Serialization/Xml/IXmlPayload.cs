using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Leopard.Serialization.Xml
{
    public interface IXmlPayload
    {
        XmlSerializerNamespaces Xmlns { get;}
    }
}
