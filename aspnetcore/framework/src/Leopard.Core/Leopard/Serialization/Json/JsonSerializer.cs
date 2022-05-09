using Newtonsoft.Json;
using System.IO;

namespace Leopard.Serialization.Json
{
    public class JsonSerializer : ISerializer
    {
        /// <summary>
        /// Converts to json string and returns
        /// </summary>
        /// <returns></returns>
        public string Serialize<TPayload>(TPayload item)
        {
            var stringWriter = new StringWriter();
            using (JsonWriter jsonWriter = new JsonTextWriter(stringWriter))
            {
                var serializer = new Newtonsoft.Json.JsonSerializer();
                serializer.Serialize(jsonWriter, item);
            }
            return stringWriter.ToString();
        }

        public TPayload Deserialize<TPayload>(string jsonString) where TPayload : class
        {
            return JsonConvert.DeserializeObject<TPayload>(jsonString);
        }
    }
}
