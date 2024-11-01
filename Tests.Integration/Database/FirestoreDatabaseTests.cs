using FluentAssertions;
using NUnit.Framework;
using SharpFire.Firebase;
using Tests.Integration.Utils;

namespace Tests.Integration.Database;

public class FirestoreDatabaseTests : FirebaseTestBase
{
    [Test]
    public async Task Get_ShouldReturnQuerySnapshot()
    {
        var firestoreRef = FirebaseApp.FirestoreDatabase;

        var collection = firestoreRef.Collection("money-easy");
        var result = await collection.GetSnapshotAsync();

        result.Documents.Should().NotBeEmpty();
    }
    
    [Test]
    public async Task GetWithPageSize_ShouldReturnQuerySnapshotWithPageLimit()
    {
        var firestoreRef = FirebaseApp.FirestoreDatabase;

        var collection = firestoreRef.Collection("money-easy").PageSize(1);
        var result = await collection.GetSnapshotAsync();

        var documents = result.Documents.ToList();
        documents.Count.Should().Be(1);
    }
    
    [Test]
    public async Task GetDocument_ShouldReturnDocumentSnapshot()
    {
        var expectedId = "WVKZCWXyVIbymQJYsSvE";
        var firestoreRef = FirebaseApp.FirestoreDatabase;
        
        var document = firestoreRef.Collection("money-easy").Document(expectedId);
        var result = await document.GetSnapshotAsync();
       
        result.Id.Should().Be(expectedId);
    }
}