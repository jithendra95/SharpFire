using FluentAssertions;
using NUnit.Framework;
using SharpFire.Firebase;

namespace Tests.Unit.Firebase;

public class FirebaseAppTests
{
    private const string MockAccessToken = "hhhggg-hgifbghdf=4378683";
    private const string MockDatabaseUrl = "https://ssd-fft.europe-west1.firebasedatabase.app";
   
    [TearDown]
    public void TearDown()
    {
        FirebaseApp.Dispose();
    }


    [Test]
    public void Create_ShouldInstantiate_RealtimeDatabaseWithAccessToken()
    {
        var appOptions =
            new AppOptions(MockAccessToken, MockDatabaseUrl);
        FirebaseApp.Create(appOptions);
        FirebaseApp.RealtimeDatabase.Should().NotBeNull();
    }

    [Test]
    public void Create_ShouldFail_WhenDatabaseUrlIsEmpty()

    {
        var appOptions =
            new AppOptions { DatabaseUrl = String.Empty, AccessToken = MockAccessToken, PathToSecretFile = "path/to/secret" };
        var action = () => FirebaseApp.Create(appOptions);
        action.Should().Throw<Exception>();
    }

    [Test]
    public void Create_ShouldFail_WhenAccessTokenAndPathToSecretIsEmpty()

    {
        var appOptions =
            new AppOptions { DatabaseUrl = MockDatabaseUrl, AccessToken = string.Empty, PathToSecretFile = string.Empty };
        var action = () => FirebaseApp.Create(appOptions);
        action.Should().Throw<Exception>();
    }

    [Test]
    public void Create_ShouldFail_WhenInvokedMultipleTimes_WithoutDisposing()
    {
        var appOptions =
            new AppOptions(MockAccessToken, MockDatabaseUrl);
        FirebaseApp.Create(appOptions);

        var action = () => FirebaseApp.Create(appOptions);
        action.Should().Throw<Exception>().WithMessage("FirebaseApp is already initialized");
    }

    [Test]
    public void RealtimeDatabase_ShouldShouldThrowException_WhenNotInitialized()
    {
        var action = () => FirebaseApp.RealtimeDatabase;
        action.Should().Throw<Exception>().WithMessage("FirebaseApp is not initialized");
    }
}