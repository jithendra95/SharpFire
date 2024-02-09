using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;
using SharpFire.Firebase;
using Tests.Integration.Utils;

namespace Tests.Integration.Database;

public class RealtimeDatabaseTests : FirebaseTestBase
{
    private const string TestEndpoint = "integration/test/";
    private string _testId;

    private record MockObject(string Name, int Age);

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _testId = await FirebaseApp.RealtimeDatabase.Post(TestEndpoint, new MockObject("John", 25));
    }

    [OneTimeTearDown]
    public void OneTimeTeardown()
    {
        FirebaseApp.RealtimeDatabase.Delete(TestEndpoint).Wait();
    }

    [Test]
    public async Task Get_ShouldReturnJsonString()
    {
        var result = await FirebaseApp.RealtimeDatabase.Get($"{TestEndpoint}{_testId}");
        
        result.Should().BeOfType<string>();
        var mockObject = JsonConvert.DeserializeObject<MockObject>(result);
        mockObject?.Name.Should().Be("John");
        mockObject?.Age.Should().Be(25);
    }
    
    [Test]
    public async Task GetWithType_ShouldReturnObject()
    {
        var result = await FirebaseApp.RealtimeDatabase.Get<MockObject>($"{TestEndpoint}{_testId}");
        result?.Name.Should().Be("John");
        result?.Age.Should().Be(25);
    }
    
    [Test]
    public async Task PosyWithType_ShouldReturnStringName()
    {
        var newObject = new MockObject("Nathan ", 30);
        var result = await FirebaseApp.RealtimeDatabase.Post<MockObject>($"{TestEndpoint}{_testId}", newObject);
        result.Should().NotBeNull();
    }
}