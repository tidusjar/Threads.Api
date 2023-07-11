using Threads.Api;

namespace Threads.Example.App
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Threads.Net API Example");

            IThreadsApi api = new ThreadsApi(new HttpClient());

            var username = "tidusjar";

            var response = await api.GetUserIdFromUserNameAsync(username);
            Console.WriteLine(response.UserId);
            var userProfile = await api.GetUserProfileAsync(username, response.UserId, response.Token);

            Console.WriteLine(userProfile?.UserName);

            var threads = await api.GetThreadsAsync(username, response.UserId, response.Token);

            var replies = await api.GetUserRepliesAsync(username, response.UserId, response.Token);

            var token = await api.GetTokenAsync("tidusjar", "pass");

        }
    }
}