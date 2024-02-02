using Newtonsoft.Json;
using SharpFire.Utils.Http;
using SharpFire.Utils.Serializer;
using static System.GC;

namespace SharpFire.Database;

public class RealtimeDatabase : IDisposable
{
    private readonly ISerializer _serializer;

    private readonly IRequestManager _requestManager;
    private string AccessToken { get; }

    public RealtimeDatabase(string accessToken, ISerializer serializer,
        IRequestManager requestManager)
    {
        _serializer = serializer;
        _requestManager = requestManager;
        AccessToken = accessToken;
    }

    public async Task<T?> Get<T>(string path)
    {
        var responseData = await Get(path);
        return JsonConvert.DeserializeObject<T>(responseData);
    }

    public async Task<string> Get(string path)
    {
        return await _requestManager.Get(RequestUri(path));
    }

    public async Task<string> Post<T>(string path, T value)
    {
        var responseData = await _requestManager.Post(RequestUri(path), _serializer.Serialize(value));
        var createdName = _serializer.Deserialize<PostResponse>(responseData);
        return createdName?.name ?? string.Empty;
    }

    public async Task<string> Post(string path, string value)
    {
        var responseData = await _requestManager.Post(RequestUri(path), new StringContent(value));
        var createdName = _serializer.Deserialize<PostResponse>(responseData);
        return createdName?.name ?? string.Empty;
    }

    public async Task<T?> Put<T>(string path, T value)
    {
        var responseData = await _requestManager.Put(RequestUri(path), _serializer.Serialize(value));
        return _serializer.Deserialize<T>(responseData);
    }

    public async Task<string> Put(string path, string value)
    {
        return await _requestManager.Put(RequestUri(path), new StringContent(value));
    }

    public async Task<T?> Patch<T>(string path, T value)
    {
        var responseData = await _requestManager.Patch(RequestUri(path), _serializer.Serialize(value));
        return JsonConvert.DeserializeObject<T>(responseData);
    }

    public async Task<string> Patch(string path, string value)
    {
        return await _requestManager.Patch(RequestUri(path), new StringContent(value));
    }

    public async Task<bool> Delete(string path)
    {
        return await _requestManager.Delete(RequestUri(path));
    }

    private string RequestUri(string path)
    {
        return $"{path}.json?auth={AccessToken}";
    }

    public void Dispose()
    {
        SuppressFinalize(this);
    }
}