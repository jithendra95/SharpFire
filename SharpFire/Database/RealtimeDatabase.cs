using System.Net;
using Newtonsoft.Json;
using static System.GC;
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
        var responseData = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(responseData);
    }

    public async Task<bool> Post<T>(string path, T value)
    {
        var response = await _client
            .PostAsync(
                $"{DatabaseUrl}/{path}.json?auth={AccessToken}", new StringContent(JsonConvert.SerializeObject(value)));
       return response.StatusCode == HttpStatusCode.OK;
    }
    
    public async Task<string> Put<T>(string path, T value)
    {
        var response = await _client
            .PutAsync(
                $"{DatabaseUrl}/{path}.json?auth={AccessToken}", new StringContent(JsonConvert.SerializeObject(value)));
        var responseData = await response.Content.ReadAsStringAsync();
        return responseData;
    }
    
    public async Task<bool> Patch<T>(string path, T value)
    {
        var response = await _client
            .PatchAsync(
                $"{DatabaseUrl}/{path}.json?auth={AccessToken}", new StringContent(JsonConvert.SerializeObject(value)));
        return response.StatusCode == HttpStatusCode.OK;
    }
    
    public async Task<bool> Delete(string path)
    {
        var response = await _client
            .DeleteAsync(
                $"{DatabaseUrl}/{path}.json?auth={AccessToken}");
        return response.StatusCode == HttpStatusCode.OK;
    }

    public void Dispose()
    {
        SuppressFinalize(this);
        _client.Dispose();
    }
}