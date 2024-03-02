using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SharpFire.Database;
using SharpFire.Utils.Http;
using SharpFire.Utils.Serializer;

namespace Tests.Unit.Database;

public class RealtimeDatabaseTests
{
    private RealtimeDatabase database;
    private ISerializer serializer;
    private IRequestManager requestManager;
    private readonly MockObject _mockObject = new("John", 25);

    private const string Id = "someID";
    private const string MockPostJsonString = """{"name": "someID"}""";
    private const string MockJsonString = """{"Name":"John","Age":25}""";
    private const string UsersEndpoint = "users/1";

    private record MockObject(string Name, int Age);

    [SetUp]
    public void OneTimeSetUp()
    {
        serializer = Substitute.For<ISerializer>();
        requestManager = Substitute.For<IRequestManager>();
        database = new RealtimeDatabase(serializer, requestManager, "token", "paramName");
    }

    [Test]
    public async Task Get_ShouldInvokeGetMethodOnRequestManager()
    {
        await database.Get(UsersEndpoint);
        await requestManager.Received(1).Get(Arg.Any<string>());
    }

    [Test]
    public async Task GetWithType_ShouldInvokeGetMethodOnRequestManagerAndDeserialize()
    {
        requestManager.Get(Arg.Any<string>()).Returns(MockJsonString);
        await database.Get<MockObject>(UsersEndpoint);
        await requestManager.Received(1).Get(Arg.Any<string>());
        serializer.Received(1).Deserialize<MockObject>(MockJsonString);
    }

    [Test]
    public async Task Post_ShouldInvokePostMethodOnRequestManagerAndReturnName()
    {
        requestManager.Post(Arg.Any<string>(), Arg.Any<StringContent>()).Returns(MockPostJsonString);

        var response = await database.Post(UsersEndpoint, MockJsonString);

        await requestManager.Received(1).Post(Arg.Any<string>(), Arg.Any<StringContent>());
        response.Should().BeOfType<string>();
    }

    [Test]
    public async Task PostWithType_ShouldInvokePostMethodOnRequestManagerDeserialize_AndReturnName()
    {
        requestManager.Post(Arg.Any<string>(), Arg.Any<StringContent>()).Returns(MockPostJsonString);

        var response = await database.Post(UsersEndpoint, _mockObject);

        await requestManager.Received(1).Post(Arg.Any<string>(), Arg.Any<StringContent>());
        response.Should().BeOfType<string>();
    }

    [Test]
    public async Task Put_ShouldInvokePutMethodOnRequestManagerAndReturnName()
    {
        requestManager.Put(Arg.Any<string>(), Arg.Any<StringContent>()).Returns(MockJsonString);

        var response = await database.Put(UsersEndpoint, MockJsonString);

        await requestManager.Received(1).Put(Arg.Any<string>(), Arg.Any<StringContent>());
        response.Should().BeOfType<string>();
    }

    [Test]
    public async Task PutWithType_ShouldInvokePutMethodOnRequestManagerDeserialize_AndReturnName()
    {
        requestManager.Put(Arg.Any<string>(), Arg.Any<StringContent>()).Returns(MockJsonString);

        await database.Put(UsersEndpoint, _mockObject);

        await requestManager.Received(1).Put(Arg.Any<string>(), Arg.Any<StringContent>());
    }

    [Test]
    public async Task Patch_ShouldInvokePatchMethodOnRequestManagerAndReturnName()
    {
        requestManager.Patch(Arg.Any<string>(), Arg.Any<StringContent>()).Returns(MockJsonString);

        var response = await database.Patch(UsersEndpoint, MockJsonString);

        await requestManager.Received(1).Patch(Arg.Any<string>(), Arg.Any<StringContent>());
        response.Should().BeOfType<string>();
    }

    [Test]
    public async Task PatchWithType_ShouldInvokePatchMethodOnRequestManagerDeserialize_AndReturnName()
    {
        requestManager.Patch(Arg.Any<string>(), Arg.Any<StringContent>()).Returns(MockJsonString);

        await database.Patch(UsersEndpoint, _mockObject);

        await requestManager.Received(1).Patch(Arg.Any<string>(), Arg.Any<StringContent>());
    }

    [TestCase(true)]
    [TestCase(false)]
    public async Task Delete_ShouldInvokeDeleteMethodOnRequestManager(bool deleteResponse)
    {
        requestManager.Delete(Arg.Any<string>()).Returns(deleteResponse);

        var response = await database.Delete(UsersEndpoint);

        await requestManager.Received(1).Delete(Arg.Any<string>());
        response.Should().Be(deleteResponse);
    }
}