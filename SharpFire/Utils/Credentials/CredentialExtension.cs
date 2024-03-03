using Google.Apis.Auth.OAuth2;

namespace SharpFire.Utils.Credentials;

public static class CredentialExtension
{
    public static GoogleCredential AddInternalScopes(this GoogleCredential credential)
    {
        return credential.CreateScoped(
            "https://www.googleapis.com/auth/cloud-platform", "https://www.googleapis.com/auth/firebase.database",
            "https://www.googleapis.com/auth/userinfo.email");
    }
}