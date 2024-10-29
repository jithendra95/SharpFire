using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SharpFire.Utils.Serializer;

public static class SnapshotDeserializationExtensions
{
    
    public static string? GetIdFromDocument(this JObject documentObject)
    {
        var name = documentObject["name"];
        return name?.ToString().Split("/").Last();
    }
    
    public static DateTime? GetDateTimeFromDocument(this JObject documentObject, string propertyName)
    {
        var time = documentObject[propertyName]?.ToString();
        return time != null ? DateTime.Parse(time) : null;
    }
    
    public static Dictionary<string, object> DeserializeFields(this Dictionary<string, object> fields)
    {
        return fields.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ExtractValueFromJson() );
    }
    
    private static object ExtractValueFromJson(this object jsonValue)
    {
        var value = jsonValue as JObject;
        if (value == null) return jsonValue;

        var propertyName = value.Properties().First().Name;
        var propertyValue = value[propertyName]?.ToString() ?? throw new InvalidDataException("Invalid document");
        switch (propertyName)
        {
            case "integerValue":
                return int.Parse(propertyValue);
            case "doubleValue":
                return double.Parse(propertyValue);
            case "booleanValue":
                return bool.Parse(propertyValue);
            case "stringValue":
            case "referenceValue":
                return propertyValue;
            case "geoPointValue":
                return GetGeoPoint(JObject.Parse(propertyValue));
            case "arrayValue":
                return GetArrayValues(JObject.Parse(propertyValue));
            case "mapValue":
                return GetMapValues(JObject.Parse(propertyValue));
            case "nullValue":
                return null;
            case "timestampValue":
                return DateTime.Parse(propertyValue);
        }
        return jsonValue;
    }
    
    private static (double latitude, double longitude) GetGeoPoint(JObject geoPoint)
    {
        var lat = geoPoint["latitude"]?.ToString() ?? throw new InvalidDataException("Invalid geoPoint");
        var lon = geoPoint["longitude"]?.ToString() ?? throw new InvalidDataException("Invalid geoPoint");
        return (double.Parse(lat), double.Parse(lon));
    }
    
    private static Dictionary<string, object> GetMapValues(JObject mapValues)
    {
        var fields = mapValues["fields"] ?? throw new InvalidDataException("Invalid document");
        return (fields.ToObject<Dictionary<string, object>>() ?? new Dictionary<string, object>())
            .DeserializeFields();
    }
    
    private static IList<object> GetArrayValues(JObject arrayValues)
    {
        var fields = arrayValues["values"] as JArray ?? throw new InvalidDataException("Invalid document");
        return fields.Select(f => f.ExtractValueFromJson()).ToList();
    }
}