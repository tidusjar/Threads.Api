using Threads.Api;
using Threads.Api.Exceptions;
using Threads.Api.Models.Response;

namespace Threads.Example.App
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Threads.Net API Example");

            IThreadsApi api = new ThreadsApi(new HttpClient());

            var username = "tidusjar";

            //var response = await api.GetUserIdFromUserNameAsync(username);
            //var userProfile = await api.GetUserProfileAsync(username, response.UserId, response.Token);
            //var threads = await api.GetThreadsAsync(username, response.UserId, response.Token);
            //var replies = await api.GetUserRepliesAsync(username, response.UserId, response.Token);
            var authToken = await api.GetTokenAsync(username, "r92g-bjLwpzzwmGZptCe");
            await api.PostAsync(username, "Hello!", authToken);

            var userNameToFollow = "zuck";
            var userToFollow = await api.GetUserIdFromUserNameAsync(userNameToFollow);
            await api.FollowAsync(userToFollow.UserId, userToFollow.Token, authToken);
        }
    }
}