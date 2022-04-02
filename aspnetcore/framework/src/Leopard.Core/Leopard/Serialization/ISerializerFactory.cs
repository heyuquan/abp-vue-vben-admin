namespace Leopard.Serialization
{
    public interface ISerializerFactory
    {
        ISerializer GetSerializer(SerializationFormat format);
    }
}
