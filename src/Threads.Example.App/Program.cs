using Threads.Api;

namespace Threads.Example.App;

internal class Program
{
    static async Task Main(string[] args)
    {
        IThreadsApi api = new ThreadsApi(new HttpClient());

        var username = "tidusjar";

        var r = await api.GetUserIdFromUserNameAsync(username);
        var userProfile = await api.GetUserProfileAsync(username, r.UserId, r.Token);
        var threads = await api.GetThreadsAsync(username, r.UserId, r.Token);
        var replies = await api.GetUserRepliesAsync(username, r.UserId, r.Token);
        var authToken = await api.LoginAsync("tidusjar", "pass");
        await api.PostAsync(username, "Hello!", authToken.Token);

        var userNameToFollow = "zuck";
        var userToFollow = await api.GetUserIdFromUserNameAsync(userNameToFollow);
        await api.FollowAsync(userToFollow.UserId, userToFollow.Token, authToken.Token);

    }
}