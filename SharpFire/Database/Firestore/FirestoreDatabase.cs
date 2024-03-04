using SharpFire.Firebase;
using SharpFire.Utils.Http;
using SharpFire.Utils.Serializer;

namespace SharpFire.Database.Firestore;

public class FirestoreDatabase
{
    private readonly ISerializer _serializer;
    private readonly IRequestManager _requestManager;

    public FirestoreDatabase(ISerializer serializer, IRequestManagerFactory requestManagerFactory)
    {
        _serializer = serializer;
        _requestManager = requestManagerFactory.GetRequestManager(FirebaseServiceEnum.Firestore);
    }
    
    public FirestoreCollection Collection(string collection)
    {
        return new FirestoreCollection(_requestManager, collection);
    }
}