using Threads.Api;

namespace Threads.Example.App;

internal class Program
{
    static async Task Main(string[] args)
    {
        IThreadsApi api = new ThreadsApi(new HttpClient());

        var username = Environment.GetEnvironmentVariable("TEST_USERNAME");

        //var r = await api.GetUserIdFromUserNameAsync(username);
        //var userProfile = await api.GetUserProfileAsync(username, r.UserId, r.Token);
        //var threads = await api.GetThreadsAsync(username, r.UserId, r.Token);
        //var replies = await api.GetUserRepliesAsync(username, r.UserId, r.Token);
        var authToken = await api.LoginAsync("tidusjar", Environment.GetEnvironmentVariable("TEST_PASSWORD"));
        //await api.PostAsync(username, "Hello!", authToken.Token);

        //var userNameToFollow = "zuck";
        var userToFollow = await api.GetUserIdFromUserNameAsync("loganpaul");
        await api.FollowAsync(userToFollow.UserId, userToFollow.Token, authToken.Token);


        var a = await api.PostImageAsync(username, "This message was posted from my C# Wrapper around the Threads API! https://github.com/tidusjar/Threads.Api", authToken.Token, new Api.Models.ImageUploadRequest
        {
            Path = "https://raw.githubusercontent.com/tidusjar/Threads.Api/main/assets/example.png"
        });

    }
}