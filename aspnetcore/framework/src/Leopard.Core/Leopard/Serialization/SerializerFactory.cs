namespace Leopard.Serialization
{
    public class SerializerFactory: ISerializerFactory
    {
        private static Json.JsonSerializer jsonSerializer;
        private static Xml.XmlSerializer xmlSerializer;

        static SerializerFactory()
        {
            jsonSerializer = new Json.JsonSerializer();
            xmlSerializer = new Xml.XmlSerializer();
        }

        public ISerializer GetSerializer(SerializationFormat format)
        {
            switch (format)
            {
                case SerializationFormat.JSON:
                    return jsonSerializer;
                default:
                case SerializationFormat.XML:
                    return xmlSerializer;
            }
        }
    }
}
