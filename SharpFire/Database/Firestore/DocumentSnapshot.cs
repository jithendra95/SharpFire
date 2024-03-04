﻿using Newtonsoft.Json.Linq;

namespace SharpFire.Database.Firestore;

public class DocumentSnapshot
{
    public string Id { get; }

    private readonly JToken? _fields;
    public readonly string? CreatedTime;
    public string? UpdatedTime;

    public DocumentSnapshot(string json)
    {
        var documentObject = JObject.Parse(json);
        Id = GetIdFromDocument(documentObject) ?? throw new InvalidDataException("Invalid document");
        _fields = documentObject["fields"] ?? throw new InvalidDataException("Invalid document");
        CreatedTime = documentObject["createTime"]?.ToString();
        UpdatedTime = documentObject["updateTime"]?.ToString();
    }
    private static string? GetIdFromDocument(JObject documentObject)
    {
        var name = documentObject["name"];
        return name?.ToString().Split("/").Last();
    }

    public Dictionary<string, object> ToDictionary()
    {
        return _fields?.ToObject<Dictionary<string, object>>() ?? new Dictionary<string, object>();
    }
}