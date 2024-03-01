namespace SharpFire.Firebase;

public record AppOptions
{
    public AppOptions(string accessToken, string databaseUrl)
    {
        AccessToken = accessToken;
        DatabaseUrl = databaseUrl;
    }

    public AppOptions()
    {
        // Empty constructor
    }

    public string? PathToSecretFile { get; init; }
    public string? SecretJson { get; init; }
    public string? AccessToken { get; init; }
    public string DatabaseUrl { get; init; }
};