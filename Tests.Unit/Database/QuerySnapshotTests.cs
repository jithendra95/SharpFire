using FluentAssertions;
using NUnit.Framework;
using SharpFire.Database.Firestore;

namespace Tests.Unit.Database;

public class QuerySnapshotTests
{
    private readonly string _jsonContent = File.ReadAllText("./Database/SampleData/QuerySnapshot.json");


    [Test]
    public void QuerySnapshot_ShouldExtractDocuments()
    {
        var querySnapshot = new QuerySnapshot(_jsonContent);
        var documents = querySnapshot.Documents;
        documents.Count().Should().Be(2);
    }

    [Test]
    public void QuerySnapshot_ShouldThrowInvalidDataResponse_ForJsonWithoutDocuments()
    {
        var act = () => new QuerySnapshot(@"{""path"": ""Invalid JSON""}").Documents;
        act.Should().Throw<InvalidDataException>();
    }
}