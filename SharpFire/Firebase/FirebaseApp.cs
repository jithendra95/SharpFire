﻿using Google.Apis.Auth.OAuth2;
using SharpFire.Database;
using SharpFire.Database.Firestore;
using SharpFire.Utils.Credentials;
using SharpFire.Utils.Http;
using SharpFire.Utils.Serializer;

namespace SharpFire.Firebase;

public static class FirebaseApp
{
    private static RealtimeDatabase? _realtimeDatabase;
    private static FirestoreDatabase? _firestore;
    
    private static readonly ISerializer Serializer = new Serializer();

    private static readonly object CreationLock = new();
   

    public static RealtimeDatabase RealtimeDatabase =>
        _realtimeDatabase ?? throw new Exception("FirebaseApp is not initialized");
    
    public static FirestoreDatabase FirestoreDatabase =>
        _firestore ?? throw new Exception("Firestore is not initialized");

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
            
            var serviceAccountHelper = new FirebaseServiceAccountHelper(appOptions);
            var requestManagerFactory = new RequestManagerFactory(appOptions, serviceAccountHelper);
            
            _realtimeDatabase = new RealtimeDatabase(Serializer, requestManagerFactory, serviceAccountHelper);
            _firestore = new FirestoreDatabase(Serializer, requestManagerFactory);
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

    public static void Dispose()
    {
        lock (CreationLock)
        {
            _realtimeDatabase?.Dispose();
            _realtimeDatabase = null;
        }
    }
}