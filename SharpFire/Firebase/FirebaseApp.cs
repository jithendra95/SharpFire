﻿using Google.Apis.Auth.OAuth2;
using SharpFire.Database;
using SharpFire.Utils.Http;
using SharpFire.Utils.Serializer;

namespace SharpFire.Firebase;

public static class FirebaseApp
{
    private static RealtimeDatabase? _realtimeDatabase;
    private static readonly ISerializer Serializer = new Serializer();

    private static readonly object CreationLock = new();

    public static RealtimeDatabase RealtimeDatabase =>
        _realtimeDatabase ?? throw new Exception("FirebaseApp is not initialized");

    /// <summary>
    /// Initializes the RealtimeDatabase with the given firebase app options.
    /// </summary>
    /// <example>
    /// FirebaseApp.Create(new AppOptions(...));
    /// </example>
    /// <param name="appOptions"></param>
    public static void Create(AppOptions appOptions)
    {
        ValidateAppOptions(appOptions);

        lock (CreationLock)
        {
            if (_realtimeDatabase != null)
                throw new Exception("FirebaseApp is already initialized");

            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(appOptions.DatabaseUrl);

            var requestManager = new RequestManager(appOptions.DatabaseUrl);
            var tokenAndParam = GetAccessTokenAndParamName(appOptions);
            _realtimeDatabase = new RealtimeDatabase(Serializer, requestManager, tokenAndParam.accesstoken,
                tokenAndParam.paramName);
        }
    }

    private static void ValidateAppOptions(AppOptions appOptions)
    {
        if (string.IsNullOrWhiteSpace(appOptions.PathToSecretFile) &&
            string.IsNullOrWhiteSpace(appOptions.SecretJson) && string.IsNullOrWhiteSpace(appOptions.AccessToken))
        {
            throw new Exception("Path to secret file and access token both cannot be empty");
        }

        if (string.IsNullOrWhiteSpace(appOptions.DatabaseUrl))
        {
            throw new Exception("Database URL cannot be empty");
        }
    }

    private static (string accesstoken, string paramName) GetAccessTokenAndParamName(AppOptions appOptions)
    {
        if (appOptions.PathToSecretFile is not null)
        {
            var credential = GoogleCredential.FromFile(appOptions.PathToSecretFile).CreateScoped(
                "https://www.googleapis.com/auth/cloud-platform", "https://www.googleapis.com/auth/firebase.database",
                "https://www.googleapis.com/auth/userinfo.email");
            var token = credential.UnderlyingCredential.GetAccessTokenForRequestAsync().GetAwaiter().GetResult();
            return (token, "access_token");
        }

        if (appOptions.SecretJson is not null)
        {
            var credential = GoogleCredential.FromJson(appOptions.SecretJson).CreateScoped(
                "https://www.googleapis.com/auth/cloud-platform", "https://www.googleapis.com/auth/firebase.database",
                "https://www.googleapis.com/auth/userinfo.email");
            var token = credential.UnderlyingCredential.GetAccessTokenForRequestAsync().GetAwaiter().GetResult();
            return (token, "access_token");
        }

        if (appOptions.AccessToken is not null)
            return (appOptions.AccessToken, "auth");

        throw new InvalidOperationException();
    }

    public static void Dispose()
    {
        lock (CreationLock)
        {
            _realtimeDatabase?.Dispose();
            _realtimeDatabase = null;
        }
    }
}