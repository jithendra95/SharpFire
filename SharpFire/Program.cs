

using SharpFire.FirebaseApp;
using SharpFire.RealtimeDatabase;

Console.WriteLine("Hello World!");

FirebaseApp.Initialize(
    @"C:\Users\Jithendra.Thenuwara\Downloads\super-pass-64e2b-firebase-adminsdk-7zxq2-b728f75e41.json");
    
    RealtimeDatabase.PrintEverything().Wait();