namespace SharpFire.Utils.Credentials;

public interface IFirebaseServiceAccountHelper
{
    string GetAccessToken();
    string GetProjectId();
    bool IsUsingServiceAccount();
}