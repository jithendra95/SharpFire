namespace SharpFire.Utils.Http;

internal class RequestManager : IRequestManager
{
    private readonly HttpClient _client;

    public RequestManager(HttpClient client)
    {
        _client = client;
    }

    public async Task<string> Get(string url)
    {
        var httpMessage = new HttpRequestMessage(HttpMethod.Get, url);
        return await SendRequest(httpMessage);
    }

    public async Task<string> Post(string url, StringContent content)
    {
        var httpMessage = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = content
        };
        return await SendRequest(httpMessage);
    }
    
    public async Task<string> Put(string url, StringContent content)
    {
        var httpMessage = new HttpRequestMessage(HttpMethod.Put, url)
        {
            Content = content
        };
        return await SendRequest(httpMessage);
    }

    public async Task<string> Patch(string url, StringContent content)
    {
        var httpMessage = new HttpRequestMessage(HttpMethod.Patch, url)
        {
            Content = content
        };
        return await SendRequest(httpMessage);
    }

    public async Task<bool> Delete(string url)
    {
       var response = await _client.DeleteAsync(url);
       return response.IsSuccessStatusCode;
    }
    
    private async Task<string> SendRequest(HttpRequestMessage httpMessage)
    {
        var response = await _client
            .SendAsync(httpMessage);
        return await response.Content.ReadAsStringAsync();
    }
    
}