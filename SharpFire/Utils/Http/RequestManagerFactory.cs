using SharpFire.Firebase;

namespace SharpFire.Utils.Http;

public class RequestManagerFactory : IRequestManagerFactory
{
    private readonly AppOptions _appOptions;

    public RequestManagerFactory(AppOptions appOptions)
    {
        _appOptions = appOptions;
    }

    public IRequestManager GetRequestManager(FirebaseServiceEnum service)
    {
        switch (service)
        {
            case FirebaseServiceEnum.RealtimeDatabase:
                return GetRealtimeDatabaseRequestManager(_appOptions);
            case FirebaseServiceEnum.Firestore:
                throw new NotImplementedException();
            default:
                throw new ArgumentOutOfRangeException(nameof(service), service, null);
        }
    }

    private IRequestManager GetRealtimeDatabaseRequestManager(AppOptions appOptions)
    {
        var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri(appOptions.DatabaseUrl.EndsWith("/")
            ? appOptions.DatabaseUrl
            : appOptions.DatabaseUrl + "/");
        return new RequestManager(httpClient);
    }
}