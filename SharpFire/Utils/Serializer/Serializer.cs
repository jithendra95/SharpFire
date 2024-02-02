using Newtonsoft.Json;

namespace SharpFire.Utils.Serializer;

public class Serializer: ISerializer
{
    public StringContent Serialize<T>(T obj)
    {
        return new StringContent(JsonConvert.SerializeObject(obj));
    }

    public T? Deserialize<T>(string json)
    {
        return JsonConvert.DeserializeObject<T>(json);
    }
}