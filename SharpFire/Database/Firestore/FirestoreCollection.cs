using SharpFire.Utils.Http;

namespace SharpFire.Database.Firestore;

public class FirestoreCollection
{
    private readonly string _path;
    private readonly IRequestManager _requestManager;
    private string _queryParams = string.Empty;

    public FirestoreCollection(IRequestManager requestManager, string path)
    {
        _requestManager = requestManager;
        _path = path;
    }

    public FirestoreDocument Document(string document)
    {
        return new FirestoreDocument(_requestManager, $"{_path}/{document}");
    }
    
    public FirestoreCollection PageSize(int size)
    {
        var pageSize= $"pageSize={size}";
        _queryParams += _queryParams == string.Empty ? pageSize : $"&{pageSize}";
        return this;
    }
    
    public FirestoreCollection OrderBy(string field, string direction = "asc")
    {
        var orderBy =  $"orderBy={field} {direction}";
        _queryParams += _queryParams == string.Empty ? orderBy : $"&{orderBy}";
        return this;
    }

    public async Task<QuerySnapshot> GetSnapshot()
    {
        var requestPath = _queryParams == string.Empty ? _path : $"{_path}?{_queryParams}";
        var response = await _requestManager.Get(requestPath);
        return new QuerySnapshot(response);
    }
}