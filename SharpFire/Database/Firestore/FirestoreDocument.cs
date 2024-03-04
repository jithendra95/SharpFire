using SharpFire.Utils.Http;

namespace SharpFire.Database.Firestore;

public class FirestoreDocument
{
    private readonly IRequestManager _requestManager;
    private readonly string _path;

    public FirestoreDocument(IRequestManager requestManager, string path)
    {
        _requestManager = requestManager;
        _path = path;
    }

    public FirestoreCollection Collection(string collection)
    {
        return new FirestoreCollection(_requestManager, $"{_path}/{collection}");
    }

    public async Task<DocumentSnapshot> GetSnapshotAsync()
    {
        var response = await _requestManager.Get(_path);
        return new DocumentSnapshot(response);
    }
}