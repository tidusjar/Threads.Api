using System.Net;
using Threads.Api.Exceptions;

namespace Threads.Api.Tests;

public class ThreadsApiTests
{

    private ThreadsApi _subject;
    private AutoMocker _mocker;

    [SetUp]
    public void Setup()
    {
        _mocker = new AutoMocker();
        _subject = _mocker.CreateInstance<ThreadsApi>();
    }

    [TestCase("")]
    [TestCase(null)]
    public void GetUserIdFromUserName_InvalidUsername(string username)
    {
        Assert.ThrowsAsync<ArgumentNullException>(async () => await _subject.GetUserIdFromUserNameAsync(username));
    }

    [Test]
    public async Task GetUserIdFromUserName_Match_Found()
    {
        _mocker.Setup<HttpClient, Task<HttpResponseMessage>>(x => x.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"props\":{\"user_id\":\"999\"}, [\"LSD\",[],{\"token\":\"LSD_yo\"},323]")
            });

        var response = await _subject.GetUserIdFromUserNameAsync("username");

        Assert.Multiple(() =>
        {
            Assert.That(response.UserId, Is.EqualTo(999));
            Assert.That(response.Token, Is.EqualTo("LSD_yo"));
        });
    }

    [Test]
    public void GetUserIdFromUserName_Match_NotFound()
    {
        _mocker.Setup<HttpClient, Task<HttpResponseMessage>>(x => x.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Nada")
            });

        Assert.ThrowsAsync<UserNotFoundException>(async () => await _subject.GetUserIdFromUserNameAsync("user"));
    }

    [TestCase(null, 1, "a")]
    [TestCase("", 1, "a")]
    [TestCase("a", 0, "a")]
    [TestCase("a", 1, "")]
    [TestCase("a", 1, null)]
    public void GetUserProfileAsyncTest_InvalidFields(string username, int userId, string token)
    {
        Assert.ThrowsAsync<ArgumentNullException>(async () => await _subject.GetUserProfileAsync(username, userId, token));
    }

    [Test]
    public async Task GetUserProfileAsyncTest()
    {
        _mocker.Setup<HttpClient, Task<HttpResponseMessage>>(x => x.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"data\": {\"userData\": " +
                "{ \"user\": { \"username\": \"unittest\", \"profile_pic_url\":\"1\", \"biography\":\"hi\" } } } }")
            });

        var response = await _subject.GetUserProfileAsync("username", 100, "token");
        Assert.Multiple(() =>
        {
            Assert.That(response.UserName, Is.EqualTo("unittest"));
            Assert.That(response.ProfilePicUrl, Is.EqualTo("1"));
            Assert.That(response.Biography, Is.EqualTo("hi"));
        });
    }
}