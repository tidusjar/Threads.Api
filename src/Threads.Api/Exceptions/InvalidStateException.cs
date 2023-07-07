using System;

namespace Threads.Api.Exceptions;

public class InvalidStateException : Exception
{
    public InvalidStateException() : base("Please call GetUserIdFromUserNameAsync first")
    {
    }
}
