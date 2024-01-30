using SharpFire.Database;

namespace SharpFire.FirebaseApp;

public static class FirebaseApp
{
    private static RealtimeDatabase? _realtimeDatabase = default;

    public static RealtimeDatabase RealtimeDatabase =>
        _realtimeDatabase ?? throw new Exception("FirebaseApp is not initialized");

    public static void Create(AppOptions appOptions)
    {
        _realtimeDatabase = new RealtimeDatabase(appOptions.AccessToken, appOptions.DatabaseUrl);
    }
}