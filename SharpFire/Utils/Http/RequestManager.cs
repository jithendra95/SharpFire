namespace SharpFire.Utils.Http;

internal class RequestManager : IRequestManager
{
    private readonly HttpClient _client;
    public RequestManager(HttpClient httpClient)
    {
        _client = httpClient;
    }

    public Task<string> Get(string url)
    {
        var httpMessage = new HttpRequestMessage(HttpMethod.Get, url);
        return SendRequest(httpMessage);
    }

    public Task<string> Post(string url, StringContent content)
    {
        var httpMessage = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = content
        };
        return SendRequest(httpMessage);
    }

    public Task<string> Put(string url, StringContent content)
    {
        var httpMessage = new HttpRequestMessage(HttpMethod.Put, url)
        {
            Content = content
        };
        return SendRequest(httpMessage);
    }

    public Task<string> Patch(string url, StringContent content)
    {
        var httpMessage = new HttpRequestMessage(HttpMethod.Patch, url)
        {
            Content = content
        };
        return SendRequest(httpMessage);
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
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(
                $"Request failed with status code: {response.StatusCode} and message {response.RequestMessage}");
        }

        return await response.Content.ReadAsStringAsync();
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}