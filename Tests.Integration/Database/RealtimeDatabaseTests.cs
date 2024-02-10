using AutoFixture;
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
    private readonly Fixture _fixture = new();

    private record MockObject(string Name, int Age);

    private record MockAgeObject(int Age);

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
        var result = await GetData(_testId);

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
    public async Task GetWithInvalidEndpoint_ShouldReturnNull()
    {
        var result = await FirebaseApp.RealtimeDatabase.Get<MockObject>("ggg");
        result.Should().BeNull();
    }

    [Test]
    public async Task Post_ShouldReturnStringNameAndPersistData()
    {
        var mockObject = CreateMock();
        var mockJsonString = JsonConvert.SerializeObject(mockObject);
        var result = await FirebaseApp.RealtimeDatabase.Post($"{TestEndpoint}", mockJsonString);

        result.Should().NotBeNull();
        ValidatePersistedData(result, "Nathan", 30);
    }


    [Test]
    public async Task PostWithType_ShouldReturnStringNameAndPersistData()
    {
        var mockObject = CreateMock();
        var result = await FirebaseApp.RealtimeDatabase.Post($"{TestEndpoint}", mockObject);

        result.Should().NotBeNull();
        ValidatePersistedData(result, mockObject.Name, mockObject.Age);
    }

    [Test]
    public async Task Put_ShouldReturnStringAndReplaceData()
    {
        var testId = _fixture.Create<string>();
        var mockObject = CreateMock();
        var mockJsonString = JsonConvert.SerializeObject(mockObject);
        
        var result = await FirebaseApp.RealtimeDatabase.Put($"{TestEndpoint}{testId}", mockJsonString);
       
        result.Should().NotBeNull();

        var savedObject = JsonConvert.DeserializeObject<MockObject>(result);
        savedObject?.Name.Should().Be(mockObject.Name);
        savedObject?.Age.Should().Be(mockObject.Age);

        ValidatePersistedData(testId, mockObject.Name, mockObject.Age);
    }

    [Test]
    public async Task PutWithType_ShouldReturnObjectAndReplaceData()
    {
        var testId = _fixture.Create<string>();
        var mockObject = CreateMock();
        
        var result = await FirebaseApp.RealtimeDatabase.Put($"{TestEndpoint}{testId}", mockObject);
        
        result.Should().NotBeNull();
        result?.Name.Should().Be(mockObject.Name);
        result?.Age.Should().Be(mockObject.Age);

        ValidatePersistedData(testId, mockObject.Name, mockObject.Age);
    }

    [Test]
    public async Task Patch_ShouldReturnPatchedDataStringAndUpdateData()
    {
        var mockObject = CreateMock();
        
        var id = await FirebaseApp.RealtimeDatabase.Post($"{TestEndpoint}", mockObject);
        
        var putAge= _fixture.Create<int>();
        var newObject = new MockAgeObject(putAge);
        
        var result = await FirebaseApp.RealtimeDatabase.Patch($"{TestEndpoint}{id}", JsonConvert.SerializeObject(newObject));
        
        result.Should().NotBeNull();
        
        var savedObject = JsonConvert.DeserializeObject<MockAgeObject>(result);
        savedObject?.Age.Should().Be(newObject.Age);

        ValidatePersistedData(_testId, mockObject.Name, newObject.Age);
    }

    [Test]
    public async Task PatchWithType_ShouldReturnPatchedDataAndUpdateData()
    {
        var mockObject = CreateMock();
        var id = await FirebaseApp.RealtimeDatabase.Post($"{TestEndpoint}", mockObject);
        
        var putAge= _fixture.Create<int>();
        var newObject = new MockAgeObject(putAge);
        var result = await FirebaseApp.RealtimeDatabase.Patch($"{TestEndpoint}{id}", newObject);
        
        result.Should().NotBeNull();
        result?.Age.Should().Be(newObject.Age);

        ValidatePersistedData(_testId, mockObject.Name, newObject.Age);
    }


    private static Task<string> GetData(string testId)
    {
        return FirebaseApp.RealtimeDatabase.Get($"{TestEndpoint}{testId}");
    }

    private static async void ValidatePersistedData(string testId, string expectedName, int expectedAge)
    {
        var result = await GetData(testId);
        var mockObject = JsonConvert.DeserializeObject<MockObject>(result);
        mockObject?.Name.Should().Be(expectedName);
        mockObject?.Age.Should().Be(expectedAge);
    }
    
    
    private MockObject CreateMock()
    {
        var name = _fixture.Create<string>();
        var age = _fixture.Create<int>();

        return new MockObject(name, age);
    }
}