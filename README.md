# SharpFire
[![Test](https://github.com/jithendra95/SharpFire/actions/workflows/build.yml/badge.svg)](https://github.com/jithendra95/SharpFire/actions/workflows/build.yml)
[![NuGet](https://img.shields.io/nuget/v/SharpFire.svg)]([https://www.nuget.org/packages/SharpFire])

This library acts as Firebase API wrapper for .NET core.
Currently only supports basic XRUD for Realtime Database but plans to support more advance querying, Firestore and more in the near future.

## Installation

The best and easiest way to add the SharpFire library to your .NET project is to use the NuGet package manager.
```csharp
Install-Package FireSharp
```

## Sample usage for Realtime Database
### Initialization
```csharp
var apiToken = Environment.GetEnvironmentVariable("API_TOKEN");
var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

FirebaseApp.Create(new AppOptions(apiToken, databaseUrl)); //Creates an instance of Realtimedatabase
```
### GET

```csharp
var response = await FirebaseApp.RealtimeDatabase.Get<User>("yourapp/users/"); //Type of User
```


### POST
Generates a unique identifier for the data created under the node specified
```csharp
var user = new User{
            name: "Test User",
            email:"test@email.com"
         }
var response = await FirebaseApp.RealtimeDatabase.Post("yourapp/users/", user); //returns the unique id of the object created.
```

### PUT
Add the data under the specified node
```csharp
var contact = new Contact{
            phone: "12223334444",
            address:"dummy address"
         }
var response = await FirebaseApp.RealtimeDatabase.Put("yourapp/users/1/contact/", contact); //returns data specified in the method call.
```

### PATCH
Change a specific value under a node without changing the other values. 
> :warning: Other values should not exist on the object, having empty values will still override the existing value.
```csharp
var newPhoneNumber = new Contact{
            phone: "12223334444"
         }
var response = await FirebaseApp.RealtimeDatabase.Patch("yourapp/users/1/contact/", newPhoneNumber); //returns object of data specified in the method call .
```

### DELETE
```csharp
var response = await FirebaseApp.RealtimeDatabase.Delete("yourapp/users/1");//Boolean response
```

# Latest release notes
## 1.0.0
- Support for Realtime database API

# Coming Soon

- Firestore API
- Subscribe to realtime updates
- Data querying for Relatime database and Firestore
- More secure Authentication using Google API's
- User Authentication API
- Firebase Storage API
