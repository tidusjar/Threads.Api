using Threads.Api;

namespace Threads.Example.App
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Threads.Net API Example");

            IThreadsApi api = new ThreadsApi(new HttpClient());

            var userId = await api.GetUserIdFromUserNameAsync("tidusjar");

        }
    }
}