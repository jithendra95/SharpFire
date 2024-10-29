using Newtonsoft.Json.Linq;
using SharpFire.Utils.Serializer;

namespace SharpFire.Database.Firestore;

public class DocumentSnapshot
{
    public string Id { get; }

    private readonly JToken? _fields;
    public readonly DateTime? CreatedTime;
    public readonly DateTime? UpdatedTime;

    public DocumentSnapshot(string json)
    {
        var documentObject = JObject.Parse(json);
        Id = documentObject.GetIdFromDocument() ?? throw new InvalidDataException("Invalid document");
        _fields = documentObject["fields"] ?? throw new InvalidDataException("Invalid document");

        CreatedTime = documentObject.GetDateTimeFromDocument("createTime");
        UpdatedTime = documentObject.GetDateTimeFromDocument("updateTime");
    }
    
    public Dictionary<string, object> ToDictionary()
    {
        return (_fields?.ToObject<Dictionary<string, object>>() ?? new Dictionary<string, object>())
            .DeserializeFields();
    }
}