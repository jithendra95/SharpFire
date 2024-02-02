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

    /// <summary>
    /// Retrieve data from the Realtime database and deserialize the output to the expected object type 
    /// </summary>
    /// <returns>T</returns>
    /// <param name="path">The path to the node in the realtime database that needs to be retrieved</param>
    /// <returns></returns>
    public async Task<T?> Get<T>(string path)
    {
        var responseData = await Get(path);
        return JsonConvert.DeserializeObject<T>(responseData);
    }

    /// <summary>
    /// Retrieve data from the Realtime database as a JSON string 
    /// </summary>
    /// <param name="path">The path to the node in the realtime database that needs to be retrieved</param>
    /// <returns></returns>
    public async Task<string> Get(string path)
    {
        return await _requestManager.Get(RequestUri(path));
    }

    /// <summary>
    /// Adds the data provided under the specified path with a unique identifier.
    /// </summary>
    /// <param name="path">The path to the node where the new data needs to be inserted</param>
    /// <param name="value">Object that will be added under the specified path </param>
    /// <returns>Generated unique identifier for the data created</returns>
    public async Task<string> Post<T>(string path, T value)
    {
        var responseData = await _requestManager.Post(RequestUri(path), _serializer.Serialize(value));
        var createdName = _serializer.Deserialize<PostResponse>(responseData);
        return createdName?.name ?? string.Empty;
    }

    /// <summary>
    /// Adds the data provided under the specified path with a unique identifier.
    /// </summary>
    /// <param name="path">The path to the node where the new data needs to be inserted</param>
    /// <param name="value">JSON string of object that will be added under the specified path </param>
    /// <returns>Generated unique identifier for the data created</returns>
    public async Task<string> Post(string path, string value)
    {
        var responseData = await _requestManager.Post(RequestUri(path), new StringContent(value));
        var createdName = _serializer.Deserialize<PostResponse>(responseData);
        return createdName?.name ?? string.Empty;
    }

    /// <summary>
    /// Adds the data on to the specified path replacing any available data on that path.
    /// </summary>
    /// <param name="path">The path to the node where the new data needs to be inserted</param>
    /// <param name="value">Object that will be added on to the specified path </param>
    /// <returns>Data specified in the method call </returns>
    public async Task<T?> Put<T>(string path, T value)
    {
        var responseData = await _requestManager.Put(RequestUri(path), _serializer.Serialize(value));
        return _serializer.Deserialize<T>(responseData);
    }

    /// <summary>
    /// Adds the data on to the specified path replacing any available data on that path.
    /// </summary>
    /// <param name="path">The path to the node where the new data needs to be inserted</param>
    /// <param name="value">JSON string of object that will be added on to the specified path </param>
    /// <returns>JSON string of data specified in the method call </returns>
    public async Task<string> Put(string path, string value)
    {
        return await _requestManager.Put(RequestUri(path), new StringContent(value));
    }
    
    /// <summary>
    /// Update the data on to the specified path without replacing any unspecified data on that path.
    /// Note: Setting attributes to empty or null will replace the data on the node.
    /// </summary>
    /// <param name="path">The path to the node where the data needs to be updated</param>
    /// <param name="value">Object that will be added on to the specified path </param>
    /// <returns>Object of data specified in the method call </returns>
    public async Task<T?> Patch<T>(string path, T value)
    {
        var responseData = await _requestManager.Patch(RequestUri(path), _serializer.Serialize(value));
        return JsonConvert.DeserializeObject<T>(responseData);
    }

    /// <summary>
    /// Update the data on to the specified path without replacing any unspecified data on that path.
    /// Note: Setting attributes to empty or null will replace the data on the node.
    /// </summary>
    /// <param name="path">The path to the node where the data needs to be updated</param>
    /// <param name="value">JSON string of object that will be added on to the specified path </param>
    /// <returns>JSON string of data specified in the method call </returns>
    public async Task<string> Patch(string path, string value)
    {
        return await _requestManager.Patch(RequestUri(path), new StringContent(value));
    }

    /// <summary>
    /// Deletes all data under the specified path
    /// </summary>
    /// <param name="path"></param>
    /// <returns>Success or Failure status</returns>
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