using Newtonsoft.Json.Linq;

namespace SharpFire.Database.Firestore;

public class QuerySnapshot
{
    private readonly string _json;
    public QuerySnapshot(string json)
    {
        _json = json;
    }

    public IEnumerable<DocumentSnapshot> Documents => GetDocuments();
    private IEnumerable<DocumentSnapshot> GetDocuments()
    {
        var collectionObject = JObject.Parse(_json);
        var documents = collectionObject["documents"];
        if (documents is null) throw new Exception("Not a valid collection");
        
        return documents.Select(document => new DocumentSnapshot(document.ToString()));
    }
    
}