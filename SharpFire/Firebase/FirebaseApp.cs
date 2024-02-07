﻿using SharpFire.Database;
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
        if (string.IsNullOrWhiteSpace(appOptions.AccessToken))
        {
            throw new Exception("Access token cannot be empty");
        }
        
        if (string.IsNullOrWhiteSpace(appOptions.DatabaseUrl))
        {
            throw new Exception("Database URL cannot be empty");
        }
        
        lock (CreationLock)
        {
            if (_realtimeDatabase != null)
                throw new Exception("FirebaseApp is already initialized");
                    
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(appOptions.DatabaseUrl);

            var requestManager = new RequestManager(httpClient, appOptions.AccessToken);
            _realtimeDatabase = new RealtimeDatabase( Serializer, requestManager);
        }
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