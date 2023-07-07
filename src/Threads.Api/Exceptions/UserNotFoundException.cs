using System;

namespace Threads.Api.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException(string username) : base($"Username '{username}' is invalid and could not be found")
    {
    }
}
