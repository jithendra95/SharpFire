using Newtonsoft.Json;
using Console = System.Console;

namespace SharpFire.Database;

public class RealtimeDatabase : IDisposable
{
    private readonly HttpClient _client = new();

    public RealtimeDatabase(string accessToken, string databaseUrl)
    {
        AccessToken = accessToken;
        DatabaseUrl = databaseUrl;
    }

    private string AccessToken { get; }
    private string DatabaseUrl { get; }

    public async Task<T?> Get<T>(string path)
    {
        var response = await _client
            .GetAsync(
                $"{DatabaseUrl}/{path}.json?auth={AccessToken}");
        var responseData = await Get(path);
        return JsonConvert.DeserializeObject<T>(responseData);
    }
    
    public async Task<string> Get(string path)
    {
        var response = await _client
            .GetAsync(
                $"{DatabaseUrl}/{path}.json?auth={AccessToken}");
        var responseData = await response.Content.ReadAsStringAsync();
        return responseData;
    }
    
    public async Task<string> GetAll()
    {
        var response = await _client
            .GetAsync(
                $"{DatabaseUrl}/.json?auth={AccessToken}");
        return await response.Content.ReadAsStringAsync();
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}