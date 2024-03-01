using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;
using SharpFire.Database.Exceptions;
using SharpFire.Firebase;
using Tests.Integration.Utils;

namespace Tests.Integration.Database;

public class RealtimeDatabaseTests : FirebaseTestBase
{
    private const string TestEndpoint = "integration/test/";
    private readonly Fixture _fixture = new();
    private record MockObject(string Name, int Age);
    private record MockAgeObject(int Age);

    [OneTimeTearDown]
    public void OneTimeTeardown()
    {
        FirebaseApp.RealtimeDatabase.Delete(TestEndpoint).Wait();
    }

    [Test]
    public async Task Get_ShouldReturnJsonString()
    {
        var mockObject = CreateMock();
        var id = await FirebaseApp.RealtimeDatabase.Post($"{TestEndpoint}", mockObject);
        
        var result = await GetData(id);

        result.Should().BeOfType<string>();
        var resultObject = JsonConvert.DeserializeObject<MockObject>(result);
        resultObject?.Name.Should().Be(mockObject.Name);
        resultObject?.Age.Should().Be(mockObject.Age);
    }

    [Test]
    public async Task GetForInvalidPath_ShouldReturnNull()
    {
        var result = await FirebaseApp.RealtimeDatabase.Get(_fixture.Create<string>());
        result.Should().BeNull();
    }

    
    [Test]
    public async Task GetWithType_ShouldReturnObject()
    {
        var mockObject = CreateMock();
        var id = await FirebaseApp.RealtimeDatabase.Post($"{TestEndpoint}", mockObject);
        
        var result = await FirebaseApp.RealtimeDatabase.Get<MockObject>($"{TestEndpoint}{id}");
        result?.Name.Should().Be(mockObject.Name);
        result?.Age.Should().Be(mockObject.Age);
    }

    [Test]
    public async Task GetWithTypeWithInvalidEndpoint_ShouldReturnNoDataException()
    {
        var act = ()=> FirebaseApp.RealtimeDatabase.Get<MockObject>(_fixture.Create<string>());
        await act.Should().ThrowAsync<NoDataFoundException>();
    }

    [Test]
    public async Task Post_ShouldReturnStringNameAndPersistData()
    {
        var mockObject = CreateMock();
        var mockJsonString = JsonConvert.SerializeObject(mockObject);
        var result = await FirebaseApp.RealtimeDatabase.Post($"{TestEndpoint}", mockJsonString);

        result.Should().NotBeNull();
        var result1 = await GetData(result);
        var mockObject1 = JsonConvert.DeserializeObject<MockObject>(result1);
        mockObject1?.Name.Should().Be(mockObject.Name);
        mockObject1?.Age.Should().Be(mockObject.Age);
    }


    [Test]
    public async Task PostWithType_ShouldReturnStringNameAndPersistData()
    {
        var mockObject = CreateMock();
        var result = await FirebaseApp.RealtimeDatabase.Post($"{TestEndpoint}", mockObject);

        result.Should().NotBeNull();
        var result1 = await GetData(result);
        var mockObject1 = JsonConvert.DeserializeObject<MockObject>(result1);
        mockObject1?.Name.Should().Be(mockObject.Name);
        mockObject1?.Age.Should().Be(mockObject.Age);
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

        var result1 = await GetData(testId);
        var mockObject1 = JsonConvert.DeserializeObject<MockObject>(result1);
        mockObject1?.Name.Should().Be(mockObject.Name);
        mockObject1?.Age.Should().Be(mockObject.Age);
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

        var result1 = await GetData(testId);
        var mockObject1 = JsonConvert.DeserializeObject<MockObject>(result1);
        mockObject1?.Name.Should().Be(mockObject.Name);
        mockObject1?.Age.Should().Be(mockObject.Age);
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

        var result1 = await GetData(id);
        var mockObject1 = JsonConvert.DeserializeObject<MockObject>(result1);
        mockObject1?.Name.Should().Be(mockObject.Name);
        mockObject1?.Age.Should().Be(newObject.Age);
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

        var result1 = await GetData(id);
        var mockObject1 = JsonConvert.DeserializeObject<MockObject>(result1);
        mockObject1?.Name.Should().Be(mockObject.Name);
        mockObject1?.Age.Should().Be(newObject.Age);
    }

    [Test]
    public async Task Delete_ShouldRemoveDataAndReturnSuccess()
    {
        var mockObject = CreateMock();
            
        var result = await FirebaseApp.RealtimeDatabase.Post($"{TestEndpoint}", mockObject);
        var response = await FirebaseApp.RealtimeDatabase.Delete($"{TestEndpoint}{result}");
        var data = await GetData(result);
            
        response.Should().BeTrue();
        data.Should().BeNull();
    }

    private static Task<string?> GetData(string testId)
    {
        return FirebaseApp.RealtimeDatabase.Get($"{TestEndpoint}{testId}");
    }


    private MockObject CreateMock()
    {
        var name = _fixture.Create<string>();
        var age = _fixture.Create<int>();

        return new MockObject(name, age);
    }
}