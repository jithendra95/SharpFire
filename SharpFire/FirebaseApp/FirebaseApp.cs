using SharpFire.Database;
using SharpFire.Utils.Http;
using SharpFire.Utils.Serializer;

namespace SharpFire.FirebaseApp;

public static class FirebaseApp
{
    private static RealtimeDatabase? _realtimeDatabase;
    private static ISerializer _serializer = new Serializer();

    public static RealtimeDatabase RealtimeDatabase =>
        _realtimeDatabase ?? throw new Exception("FirebaseApp is not initialized");

    public static void Create(AppOptions appOptions)
    {
        var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri(appOptions.DatabaseUrl);

        var requestManager = new RequestManager(httpClient);
        _realtimeDatabase = new RealtimeDatabase(appOptions.AccessToken, _serializer, requestManager);
    }
}