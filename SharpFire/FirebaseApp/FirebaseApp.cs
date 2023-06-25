using Google.Apis.Auth.OAuth2;
namespace SharpFire.FirebaseApp;

public static class FirebaseApp
{
    public static ServiceAccountCredential? ServiceAccountCredential { get; private set; }
    
    public static void Initialize(string pathToJsonFile)
    {
        var googleCredentials = GoogleCredential.FromFile(pathToJsonFile);
        ServiceAccountCredential = googleCredentials.UnderlyingCredential as ServiceAccountCredential;
    }
}