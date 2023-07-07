// See https://aka.ms/new-console-template for more information
using Threads;

Console.WriteLine("Hello, World!");

var api = new ThreadsApi();

var response = await api.GetUserIdFromUserName("tidusjar");

Console.WriteLine(response);