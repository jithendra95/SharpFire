using NUnit.Framework;
using SharpFire.Firebase;

namespace Tests.Integration.Utils;

public class FirebaseTestBase
{
    [OneTimeSetUp]
    public void SetUp()
    {
        var pathToSecret = Environment.GetEnvironmentVariable("PATH_TO_SECRET") ?? string.Empty;
        var secretJson = Environment.GetEnvironmentVariable("SECRET_JSON") ?? string.Empty;
        var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL") ?? string.Empty;


        FirebaseApp.Create(string.IsNullOrEmpty(pathToSecret)
            ? new AppOptions { SecretJson = secretJson, DatabaseUrl = databaseUrl }
            : new AppOptions { PathToSecretFile = pathToSecret, DatabaseUrl = databaseUrl });
    }
    
    [OneTimeTearDown]
    public void TearDown()
    {
        FirebaseApp.Dispose();
    }
}