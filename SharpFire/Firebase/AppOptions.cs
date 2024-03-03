namespace SharpFire.Firebase;

public record AppOptions
{
    [Obsolete(
        "Use object initializer syntax instead of this constructor. This constructor will be removed in the next major version.")]
    public AppOptions(string accessToken, string databaseUrl)
    {
        AccessToken = accessToken;
        DatabaseUrl = databaseUrl;
    }

    public AppOptions()
    {
        // Empty constructor in because the parametered constructor is obsolete and we want to have a way to us the object initializer syntax
    }

    public string? PathToSecretFile { get; init; }
    public string? SecretJson { get; init; }

    [Obsolete("Use Service account secret file instead by setting the PathToSecretFile property")]
    public string? AccessToken { get; init; }

    public string DatabaseUrl { get; init; }
};