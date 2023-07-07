using System;

namespace Threads.Api;

public class UserNotFoundException : Exception
{
	public UserNotFoundException(string username) : base($"Username '{username}' is invalid and could not be found")
	{
	}
}
