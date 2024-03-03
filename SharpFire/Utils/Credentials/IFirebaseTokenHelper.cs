namespace SharpFire.Utils.Credentials;

public interface IFirebaseTokenHelper
{
    string GetAccessToken();
    bool IsUsingServiceAccount();
}