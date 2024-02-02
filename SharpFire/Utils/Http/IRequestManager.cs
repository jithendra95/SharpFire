namespace SharpFire.Utils.Http;

public interface IRequestManager
{
    Task<string> Get(string url);
    Task<string> Post(string url, StringContent content);
    Task<string> Put(string url, StringContent content);
    Task<string> Patch(string url, StringContent content);
    Task<bool> Delete(string url);
}