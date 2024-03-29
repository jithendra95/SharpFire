﻿using FluentAssertions;
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

        var document = firestoreRef.Collection("money-easy");
        var result = await document.GetSnapshotAsync();

        result.Documents.Count().Should().Be(2);
    }
    
    [Test]
    public async Task GetWithPageSize_ShouldReturnCollectionSnapshotWithPageLimit()
    {
        var firestoreRef = FirebaseApp.FirestoreDatabase;

        var document = firestoreRef.Collection("money-easy").PageSize(1);
        var result = await document.GetSnapshotAsync();

        var documents = result.Documents.ToList();
        var docFields= documents.First().ToDictionary();
        documents.Count().Should().Be(1);
    }
}