using SharpFire.Firebase;

namespace Tests.Integration.Utils;

public class FirebaseTestBase
{
    protected FirebaseTestBase()
    {
        var apiToken = Environment.GetEnvironmentVariable("API_TOKEN") ?? string.Empty;
        var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL") ?? string.Empty;
        
        if(string.IsNullOrEmpty(apiToken) || string.IsNullOrEmpty(databaseUrl))
            throw new Exception("API_TOKEN and DATABASE_URL environment variables must be set");
        
        FirebaseApp.Create(new AppOptions(apiToken, databaseUrl));
    }
}