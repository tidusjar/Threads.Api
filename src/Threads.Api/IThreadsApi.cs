using System.Threading;
using System.Threading.Tasks;
using Threads.Api.Models;
using Threads.Api.Models.Response;

namespace Threads.Api;

public interface IThreadsApi
{
    /// <summary>
    /// Returns the User Id for the specified username
    /// </summary>
    /// <param name="username">Username of the user</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns><see cref="UserIdResponse"/></returns>
    Task<UserIdResponse> GetUserIdFromUserNameAsync(string username, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the user profile for the specified userid and username.
    /// </summary>
    /// <param name="username">Username of the user</param>
    /// <param name="userId">UserId for the user from <see cref="GetUserIdFromUserNameAsync"/></param>
    /// <param name="token">Token returned from <see cref="GetUserIdFromUserNameAsync"/></param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns><see cref="User"/></returns>
    Task<User?> GetUserProfileAsync(string username, int userId, string token, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the list of threads for the specified user
    /// </summary>
    /// <param name="username">Username of the user</param>
    /// <param name="userId">UserId for the user from <see cref="GetUserIdFromUserNameAsync"/></param>
    /// <param name="token">Token returned from <see cref="GetUserIdFromUserNameAsync"/></param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns><see cref="UserThreads"/></returns>
    Task<UserThreads> GetThreadsAsync(string username, int userId, string token, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the replies on threads for the user
    /// </summary>
    /// <param name="username">Username of the user</param>
    /// <param name="userId">UserId for the user from <see cref="GetUserIdFromUserNameAsync"/></param>
    /// <param name="token">Token returned from <see cref="GetUserIdFromUserNameAsync"/></param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns><see cref="UserReplies"/></returns>
    Task<UserReplies> GetUserRepliesAsync(string username, int userId, string token, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the authentication token for a user to perform user based actions
    /// </summary>
    /// <param name="username">Username</param>
    /// <param name="password">Password to authenticate the user</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns>a authentication token</returns>
    Task<string> GetTokenAsync(string username, string password, CancellationToken cancellationToken = default);
}
