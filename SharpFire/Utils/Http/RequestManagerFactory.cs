using System.Net.Http.Headers;
using SharpFire.Firebase;
using SharpFire.Utils.Credentials;

namespace SharpFire.Utils.Http;

public class RequestManagerFactory : IRequestManagerFactory
{
    private readonly AppOptions _appOptions;
    private readonly IFirebaseServiceAccountHelper _serviceAccountHelper;

    public RequestManagerFactory(AppOptions appOptions, IFirebaseServiceAccountHelper serviceAccountHelper)
    {
        _appOptions = appOptions;
        _serviceAccountHelper = serviceAccountHelper;
    }

    public IRequestManager GetRequestManager(FirebaseServiceEnum service)
    {
        return service switch
        {
            FirebaseServiceEnum.RealtimeDatabase => GetRealtimeDatabaseRequestManager(),
            FirebaseServiceEnum.Firestore => GetFirestoreRequestManager(),
            _ => throw new ArgumentOutOfRangeException(nameof(service), service, null)
        };
    }

    private IRequestManager GetRealtimeDatabaseRequestManager()
    {
        var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri(_appOptions.DatabaseUrl.EndsWith("/")
            ? _appOptions.DatabaseUrl
            : _appOptions.DatabaseUrl + "/");
        return new RequestManager(httpClient);
    }
    
    private IRequestManager GetFirestoreRequestManager()
    {
        var projectId = _serviceAccountHelper.GetProjectId();
        var firestoreApiUrl = $"https://firestore.googleapis.com/v1beta1/projects/{projectId}/databases/(default)/documents/";
        var token = _serviceAccountHelper.GetAccessToken();
        
        var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri(firestoreApiUrl);
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        return new RequestManager(httpClient);
    }
}