using SharpFire.Database;

namespace SharpFire.FirebaseApp;

public static class FirebaseApp
{
    private static RealtimeDatabase? _realtimeDatabase;
    private static HttpClient? _httpClient = default;

    public static RealtimeDatabase RealtimeDatabase =>
        _realtimeDatabase ?? throw new Exception("FirebaseApp is not initialized");

    public static void Create(AppOptions appOptions)
    {
        _httpClient = new HttpClient();
        _realtimeDatabase = new RealtimeDatabase(appOptions.AccessToken, appOptions.DatabaseUrl, _httpClient);
        
    }
}