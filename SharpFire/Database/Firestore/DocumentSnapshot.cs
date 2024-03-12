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
        Id = GetIdFromDocument(documentObject) ?? throw new InvalidDataException("Invalid document");
        _fields = documentObject["fields"] ?? throw new InvalidDataException("Invalid document");

        CreatedTime = GetDateTimeFromDocument(documentObject, "createTime");
        UpdatedTime = GetDateTimeFromDocument(documentObject, "updateTime");
    }

    private DateTime? GetDateTimeFromDocument(JObject documentObject, string propertyName)
    {
        var time = documentObject[propertyName]?.ToString();
        return time != null ? DateTime.Parse(time) : null;
    }

    private static string? GetIdFromDocument(JObject documentObject)
    {
        var name = documentObject["name"];
        return name?.ToString().Split("/").Last();
    }

    public Dictionary<string, object> ToDictionary()
    {
        return (_fields?.ToObject<Dictionary<string, object>>() ?? new Dictionary<string, object>())
            .DeserializeFields();
    }
}