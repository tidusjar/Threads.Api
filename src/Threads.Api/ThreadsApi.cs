﻿using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Threads.Api;

public interface IThreadsApi
{
    Task<int> GetUserIdFromUserNameAsync(string username, CancellationToken cancellationToken = default);
}

public class ThreadsApi : IThreadsApi
{
    private readonly HttpClient _client;
    private const string _url = "https://www.threads.net/";

    private string FbLSDToken { get; set; } = string.Empty;

    public ThreadsApi(HttpClient httpClient)
    {
        _client = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<int> GetUserIdFromUserNameAsync(string username, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentNullException(nameof(username));
        }

        var request = new HttpRequestMessage(HttpMethod.Get, $"{_url}@{username}");
        GetDefaultHeaders(username, request);

        request.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
        request.Headers.Add("Accept-Language", "ko,en;q=0.9,ko-KR;q=0.8,ja;q=0.7");
        request.Headers.Add("Pragma", "no-cache");
        request.Headers.Add("Referer", $"https://www.threads.net/@${username}");
        request.Headers.Add("Sec-Detch-Dest", "document");
        request.Headers.Add("Sec-Detch-Mode", "navigate");
        request.Headers.Add("Sec-Detch-Site", "cross-site");
        request.Headers.Add("Sec-Detch-User", "?1");
        request.Headers.Add("Upgrade-Insecure-Requests", "1");
        request.Headers.Add("X-asbd-id", "");
        request.Headers.Add("X-fb-lsd", "");
        request.Headers.Add("X-ig-app-id", "");


        request.Headers.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("PostmanRuntime", "7.32.3"));

        var response = await _client.SendAsync(request, cancellationToken).ConfigureAwait(false);
        var text = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        text = Regex.Replace(text, @"\s", "");
        text = Regex.Replace(text, @"\n", "");

        string userID = Regex.Match(text, @"""props"":{""user_id"":""(\d+)""}")?.Groups[1].Value;
        string lsdToken = Regex.Match(text, @"""LSD"",\[\],{""token"":""(\w+)""},\d+\]")?.Groups[1].Value;
        FbLSDToken = lsdToken;

        if (int.TryParse(userID, out int value))
        {
            return value;
        }

        throw new UserNotFoundException(username);
    }

    private void GetDefaultHeaders(string username, HttpRequestMessage request)
    {
        request.Headers.Add("Authority", "www.threads.net");
        request.Headers.Add("Cache-Control", "no-cache");
        request.Headers.Add("Origin", "https://www.threads.net");
        request.Headers.Add("x-fb-lsd", this.FbLSDToken);
        request.Headers.Add("Accept", "*/*");
    }

}