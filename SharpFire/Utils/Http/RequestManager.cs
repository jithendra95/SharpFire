namespace SharpFire.Utils.Http;

internal class RequestManager : IRequestManager
{
    private readonly HttpClient _client;
    private readonly string _authParameter;
    private string AccessToken { get; }
    
    public RequestManager(HttpClient client, string accessToken, string authParameter = "auth")
    {
        _client = client;
        _authParameter = authParameter;
        AccessToken = accessToken;
    }

    public Task<string> Get(string url)
    {
        var httpMessage = new HttpRequestMessage(HttpMethod.Get, RequestUri(url));
        return SendRequest(httpMessage);
    }

    public Task<string> Post(string url, StringContent content)
    {
        var httpMessage = new HttpRequestMessage(HttpMethod.Post, RequestUri(url))
        {
            Content = content
        };
        return SendRequest(httpMessage);
    }

    public Task<string> Put(string url, StringContent content)
    {
        var httpMessage = new HttpRequestMessage(HttpMethod.Put, RequestUri(url))
        {
            Content = content
        };
        return SendRequest(httpMessage);
    }

    public Task<string> Patch(string url, StringContent content)
    {
        var httpMessage = new HttpRequestMessage(HttpMethod.Patch, RequestUri(url))
        {
            Content = content
        };
        return SendRequest(httpMessage);
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
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Request failed with status code: " + response.StatusCode);
        }

        return await response.Content.ReadAsStringAsync();
    }
    
    private string RequestUri(string path) => $"{path}.json?{_authParameter}={AccessToken}";
    
    public void Dispose()
    {
        _client.Dispose();
    }
}