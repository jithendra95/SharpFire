namespace SharpFire.Utils.Http;

internal class RequestManager : IRequestManager
{
    private readonly HttpClient _client;
    private string AccessToken { get; }
    
    public RequestManager(HttpClient client, string accessToken)
    {
        _client = client;
        AccessToken = accessToken;
    }

    public async Task<string> Get(string url)
    {
        var httpMessage = new HttpRequestMessage(HttpMethod.Get, RequestUri(url));
        return await SendRequest(httpMessage);
    }

    public async Task<string> Post(string url, StringContent content)
    {
        var httpMessage = new HttpRequestMessage(HttpMethod.Post, RequestUri(url))
        {
            Content = content
        };
        return await SendRequest(httpMessage);
    }
    
    public async Task<string> Put(string url, StringContent content)
    {
        var httpMessage = new HttpRequestMessage(HttpMethod.Put, RequestUri(url))
        {
            Content = content
        };
        return await SendRequest(httpMessage);
    }

    public async Task<string> Patch(string url, StringContent content)
    {
        var httpMessage = new HttpRequestMessage(HttpMethod.Patch, RequestUri(url))
        {
            Content = content
        };
        return await SendRequest(httpMessage);
    }

    public async Task<bool> Delete(string url)
    {
       var response = await _client.DeleteAsync(RequestUri(url));
       return response.IsSuccessStatusCode;
    }
    
    private async Task<string> SendRequest(HttpRequestMessage httpMessage)
    {
        var response = await _client
            .SendAsync(httpMessage);
        return await response.Content.ReadAsStringAsync();
    }
    
    private string RequestUri(string path)
    {
        return $"{path}.json?auth={AccessToken}";
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}