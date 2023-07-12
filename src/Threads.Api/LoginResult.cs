namespace Threads.Api;

public partial class ThreadsApi
{
    public class LoginResult
    {
        public string Token { get; set; }
        public string UserId { get; set; }
    }
}