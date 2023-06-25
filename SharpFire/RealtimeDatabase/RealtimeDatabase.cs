using Console = System.Console;

namespace SharpFire.RealtimeDatabase;

using SharpFire.FirebaseApp;

public static class RealtimeDatabase
{
    public static async Task PrintEverything()
    {
        if (FirebaseApp.ServiceAccountCredential == null)
            throw new Exception("FirebaseApp is not initialized");
        var accessToken = await FirebaseApp.ServiceAccountCredential.GetAccessTokenForRequestAsync("https://www.googleapis.com/auth/firebase");
        
        if (accessToken == null)
            throw new Exception("Failed to get access token for request");

        using var client = new HttpClient();
        var response = await client
            .GetAsync(
                $"https://super-pass-64e2b-default-rtdb.europe-west1.firebasedatabase.app/money-easy/categories.json?auth={accessToken}");
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        Console.WriteLine(responseBody);
    }
}