namespace SharpFire.Firebase;

public class AppOptions
{
    public AppOptions(string accessToken, string databaseUrl)
    {
        AccessToken = accessToken;
        DatabaseUrl = databaseUrl;
    }

    public string AccessToken { get; }
    public string DatabaseUrl { get; }
}