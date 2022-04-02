namespace Leopard.Serialization
{
    public interface ISerializer
    {
        string Serialize<TPayload>(TPayload item);

        TPayload Deserialize<TPayload>(string content);
    }
}
