namespace SharpFire.Utils.Serializer;

public interface ISerializer
{
    public StringContent Serialize<T>(T obj);
    
    public T? Deserialize<T>(string json);
}