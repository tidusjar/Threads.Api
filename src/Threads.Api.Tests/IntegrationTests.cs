namespace Threads.Api.Tests;

[TestFixture]
[Ignore("yeah")]
public class IntegrationTests
{
    private ThreadsApi _subject;

    private readonly string? _username = Environment.GetEnvironmentVariable("TEST_USERNAME");
    private readonly string? _password = Environment.GetEnvironmentVariable("TEST_PASSWORD");

    [SetUp]
    public void Setup()
    {
        _subject = new ThreadsApi(new HttpClient());
        if (string.IsNullOrEmpty(_username))
        {
            throw new ArgumentNullException(nameof(_username));
        }

        if (string.IsNullOrEmpty(_password))
        {
            throw new ArgumentNullException(nameof(_password));
        }
    }

    [Test]
    [Ignore("Locks me out")]
    public async Task Login_Test()
    {
        var result = await _subject.LoginAsync(_username, _password);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Token, Is.Not.Null.Or.Empty);
            Assert.That(result.UserId, Is.Not.Null.Or.Empty);
        });
    }

    [Test]
    [Ignore("Hit and miss")]
    public async Task GetUserIdFromUserName_Test()
    {
        var result = await _subject.GetUserIdFromUserNameAsync(_username);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Token, Is.Not.Null);
            Assert.That(result.UserId, Is.EqualTo(3897985));
        });
    }
}
