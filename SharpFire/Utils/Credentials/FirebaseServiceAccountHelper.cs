using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json.Linq;
using SharpFire.Firebase;

namespace SharpFire.Utils.Credentials;

public class FirebaseServiceAccountHelper : IFirebaseServiceAccountHelper
{
    private readonly AppOptions _appOptions;

    public FirebaseServiceAccountHelper(AppOptions appOptions)
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

    public string GetProjectId()
    {
        if (_appOptions.PathToSecretFile is not null)
        {
            var jsonContent = File.ReadAllText(_appOptions.PathToSecretFile);
            var jsonObject = JObject.Parse(jsonContent);
            return GetProjectId(jsonObject);    
        }
        
        if (_appOptions.SecretJson is not null)
        {
            var jsonObject = JObject.Parse(_appOptions.SecretJson);
            return GetProjectId(jsonObject);
        }
        
        throw new Exception("Project Id cannot be retrieved");
    }

    private static string GetProjectId(JObject jsonObject)
    {
        return jsonObject["project_id"]?.ToString() ?? throw new Exception("Project Id not found in the secret file");
    }

    public bool IsUsingServiceAccount()
    {
        return _appOptions.PathToSecretFile is not null || _appOptions.SecretJson is not null;
    }
}