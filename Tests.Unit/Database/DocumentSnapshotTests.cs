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
        
        fields["name"].Should().Be("Jithendra");
        fields["extra"].Should().Be(1.0989);
        fields["isMarried"].Should().Be(true);
        fields["age"].Should().Be(20);
        fields["pathToParent"].Should().Be("projects/super-pass-64e2b/databases/(default)/documents/money-easy/Zw8MTlsNs1gXBSz3U5Qj");
        fields["myNull"].Should().BeNull();
    }
    
    [Test]
    public void DocumentSnapshot_ShouldExtractArray()
    {
        var documentSnapshot = new DocumentSnapshot(_jsonContent);
        var fields = documentSnapshot.ToDictionary();
        
        var myArray = fields["myArray"] as List<object>;
        myArray.Should().BeEquivalentTo(["dwsdf", "wefwef"]);
    }
    
    [Test]
    public void DocumentSnapshot_ShouldExtractGeoPoint()
    {
        var documentSnapshot = new DocumentSnapshot(_jsonContent);
        var fields = documentSnapshot.ToDictionary();
        
        var location = fields["myLats"] as  (double latitude, double longitude)?;
        location?.latitude.Should().Be(12.111);
        location?.longitude.Should().Be(45.112);
    }
    
    [Test]
    public void DocumentSnapshot_ShouldExtractMap()
    {
        var documentSnapshot = new DocumentSnapshot(_jsonContent);
        var fields = documentSnapshot.ToDictionary();
        
        var myMap = fields["myMap"] as Dictionary<string, object>;
        myMap!["newKey"].Should().Be(11);
        myMap["myKey"].Should().Be("Hello");
    }
    
    [Test]
    public void DocumentSnapshot_ShouldThrowInvalidDataResponse_ForJsonWithoutFieldsOrId()
    {
        var act = ()=> new DocumentSnapshot(@"{""path"": ""Invalid JSON""}");
        act.Should().Throw<InvalidDataException>();
    }
    
}