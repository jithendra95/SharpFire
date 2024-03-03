using SharpFire.Utils.Http;

namespace SharpFire.Database;

public class FirestoreCollection
{
    private readonly string _path;
    private readonly IRequestManager _requestManager;

    public FirestoreCollection(IRequestManager requestManager, string path)
    {
        _requestManager = requestManager;
        _path = path;
    }

    public FirestoreDocument Document(string document)
    {
        return new FirestoreDocument(_requestManager, $"{_path}/{document}");
    }
}