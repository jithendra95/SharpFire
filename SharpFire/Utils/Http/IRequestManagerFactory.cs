using SharpFire.Firebase;

namespace SharpFire.Utils.Http;

public interface IRequestManagerFactory
{
    IRequestManager GetRequestManager(FirebaseServiceEnum service);
}