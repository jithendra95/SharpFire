using FluentAssertions;
using NUnit.Framework;
using SharpFire.Firebase;
using Tests.Integration.Utils;

namespace Tests.Integration.Database;

public class FirestoreDatabaseTests : FirebaseTestBase
{
    [Test]
    public async Task Get_ShouldReturnJsonString()
    {
        var firestoreRef = FirebaseApp.FirestoreDatabase;

        var document = firestoreRef.Collection("money-easy").PageSize(1).OrderBy("Name");
        var result = await document.GetSnapshot();

        result.Should().BeOfType<string>();
    }
}