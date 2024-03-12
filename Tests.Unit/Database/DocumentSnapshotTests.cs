using FluentAssertions;
using NUnit.Framework;
using SharpFire.Database.Firestore;

namespace Tests.Unit.Database;

public class DocumentSnapshotTests
{
    private readonly string _jsonContent = File.ReadAllText("./Database/SampleData/DocumentSnapshot.json");
    [Test]
    public void DocumentSnapshot_ShouldExtractMetaData()
    {
        var documentSnapshot = new DocumentSnapshot(_jsonContent);
        documentSnapshot.Id.Should().Be("WVKZCWXyVIbymQJYsSvE");
        documentSnapshot.CreatedTime.Should().NotBeNull();
        documentSnapshot.UpdatedTime.Should().NotBeNull();
    }
    
    [Test]
    public void DocumentSnapshot_ShouldExtractFields()
    {
        var documentSnapshot = new DocumentSnapshot(_jsonContent);
        var fields = documentSnapshot.ToDictionary();
        
        fields.Should().ContainKey("name");
        fields.Should().ContainKey("isMarried");
        fields.Should().ContainKey("age");
        fields.Should().ContainKey("pathToParent");
    }
    
    [Test]
    public void DocumentSnapshot_ShouldThrowInvalidDataResponse_ForJsonWithoutFieldsOrId()
    {
        var act = ()=> new DocumentSnapshot(@"{""path"": ""Invalid JSON""}");
        act.Should().Throw<InvalidDataException>();
    }
    
}