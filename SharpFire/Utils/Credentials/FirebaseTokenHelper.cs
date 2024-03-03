using Google.Apis.Auth.OAuth2;
using SharpFire.Firebase;

namespace SharpFire.Utils.Credentials;

public class FirebaseTokenHelper : IFirebaseTokenHelper
{
    private readonly AppOptions _appOptions;

    public FirebaseTokenHelper(AppOptions appOptions)
    {
        _appOptions = appOptions;
    }
    
    public string GetAccessToken()
    {
        if (_appOptions.PathToSecretFile is not null)
        {
            var credential = GoogleCredential.FromFile(_appOptions.PathToSecretFile).AddInternalScopes();
            return credential.UnderlyingCredential.GetAccessTokenForRequestAsync().GetAwaiter().GetResult();
        }

        if (_appOptions.SecretJson is not null)
        {
            var credential = GoogleCredential.FromJson(_appOptions.SecretJson).AddInternalScopes();
            return credential.UnderlyingCredential.GetAccessTokenForRequestAsync().GetAwaiter().GetResult();
        }

        if (_appOptions.AccessToken is not null)
        {
            return _appOptions.AccessToken;
        }

        throw new Exception("No valid credentials found");
    }
    
    public bool IsUsingServiceAccount()
    {
        return _appOptions.PathToSecretFile is not null || _appOptions.SecretJson is not null;
    }
}