using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Threads.Api.Exceptions;
using Threads.Api.Models;
using Threads.Api.Models.Response;

namespace Threads.Api;

public class ThreadsApi : IThreadsApi
{
    private readonly HttpClient _client;
    private const string _url = "https://www.threads.net/";
    private const string _graphUrl = "https://www.threads.net/api/graphql";
    private const string BaseApiUrl = "https://i.instagram.com/api/v1";
    private readonly string LoginUrl = $"{BaseApiUrl}/bloks/apps/com.bloks.www.bloks.caa.login.async.send_login_request/";

    public ThreadsApi(HttpClient httpClient)
    {
        _client = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    /// <inheritdoc/>
    public async Task<UserIdResponse> GetUserIdFromUserNameAsync(string username, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentNullException(nameof(username));
        }

        var request = new HttpRequestMessage(HttpMethod.Get, $"{_url}@{username}");
        GetDefaultHeaders(token, request);

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

        var response = await _client.SendAsync(request, cancellationToken).ConfigureAwait(false);
        var text = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        text = Regex.Replace(text, @"\s", "");
        text = Regex.Replace(text, @"\n", "");

        string userID = Regex.Match(text, @"""props"":{""user_id"":""(\d+)""}")?.Groups[1].Value;
        string lsdToken = Regex.Match(text, @"""LSD"",\[\],{""token"":""(\w+)""},\d+\]")?.Groups[1].Value;

        if (string.IsNullOrEmpty(lsdToken))
        {
            throw new UserNotFoundException(username);
        }

        if (int.TryParse(userID, out int value))
        {
            return new UserIdResponse
            {
                Token = lsdToken,
                UserId = value
            };
        }

        throw new UserNotFoundException(username);
    }


    /// <inheritdoc/>
    public async Task<User?> GetUserProfileAsync(string username, int userId, string token, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(username))
        {
            throw new ArgumentNullException(username);
        }
        if (string.IsNullOrWhiteSpace(token) || userId <= 0)
        {
            throw new ArgumentNullException(token);
        }

        var param = new Dictionary<string, string> {
            { "lsd", token },
            { "variables", $"{{\"userID\":\"{userId}\"}}" },
            { "doc_id", "23996318473300828" },
        };

        var request = new HttpRequestMessage(HttpMethod.Post, new Uri(QueryHelpers.AddQueryString(_graphUrl, param)));
        GetDefaultHeaders(token, request);
        request.Headers.Add("x-fb-friendly-name", "BarcelonaProfileRootQuery");

        var response = await _client.SendAsync(request, cancellationToken).ConfigureAwait(false);
        var stream = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        var profile = JsonSerializer.Deserialize<UserProfile>(stream);

        return profile?.Data?.UserData?.User;
    }


    /// <inheritdoc/>
    public async Task<UserThreads> GetThreadsAsync(string username, int userId, string token, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(username))
        {
            throw new ArgumentNullException(username);
        }
        if (string.IsNullOrWhiteSpace(token) || userId <= 0)
        {
            throw new ArgumentNullException(token);
        }

        var param = new Dictionary<string, string> {
            { "lsd", token },
            { "variables", $"{{\"userID\":\"{userId}\"}}" },
            { "doc_id", "6232751443445612" },
        };

        var request = new HttpRequestMessage(HttpMethod.Post, new Uri(QueryHelpers.AddQueryString(_graphUrl, param)));
        GetDefaultHeaders(token, request);
        request.Headers.Add("x-fb-friendly-name", "BarcelonaProfileThreadsTabQuery");

        var response = await _client.SendAsync(request, cancellationToken).ConfigureAwait(false);
        var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
        var threads = await JsonSerializer.DeserializeAsync<UserThreads>(stream).ConfigureAwait(false);
        return threads;
    }


    /// <inheritdoc/>
    public async Task<UserReplies> GetUserRepliesAsync(string username, int userId, string token, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(username))
        {
            throw new ArgumentNullException(username);
        }
        if (string.IsNullOrWhiteSpace(token) || userId <= 0)
        {
            throw new ArgumentNullException(token);
        }

        var param = new Dictionary<string, string> {
            { "lsd", token },
            { "variables", $"{{\"userID\":\"{userId}\"}}" },
            { "doc_id", "6684830921547925" },
        };

        var request = new HttpRequestMessage(HttpMethod.Post, new Uri(QueryHelpers.AddQueryString(_graphUrl, param)));
        GetDefaultHeaders(token, request);
        request.Headers.Add("x-fb-friendly-name", "BarcelonaProfileThreadsTabQuery");
        var response = await _client.SendAsync(request, cancellationToken).ConfigureAwait(false);
        var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
        var replies = await JsonSerializer.DeserializeAsync<UserReplies>(stream).ConfigureAwait(false);
        return replies;
    }


    /// <inheritdoc/>
    public async Task<string> GetTokenAsync(string username, string password, CancellationToken cancellationToken = default)
    {
        var deviceId = "android-1vp2aultsmo00000";

        var body = JsonSerializer.Serialize(new
        {
            client_input_params = new
            {
                password = password,
                contact_point = username,
                device_id = deviceId
            },
            server_params = new
            {
                credential_type = "password",
                device_id = deviceId,
            }
        });
        var blockVersion = "5f56efad68e1edec7801f630b5c122704ec5378adbee6609a448f105f34a9c73";

        var bkClientContext = JsonSerializer.Serialize(new
        {
            bloks_version = blockVersion,
            styles_id = "instagram"
        });

        var request = new HttpRequestMessage(HttpMethod.Post, new Uri(LoginUrl));
        var content = new StringContent($"params={body}&bk_client_context={bkClientContext}&bloks_versioning_id={blockVersion}");

        GetAppHeaders(request);
        request.Content = content;
        request.Content.Headers.Clear();
        request.Content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
        request.Content.Headers.Add("Response-Type", "text");

        var response = await _client.SendAsync(request, cancellationToken).ConfigureAwait(false);
        var data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        var token = data.Split(new[] { "Bearer IGT:2:" }, StringSplitOptions.None)[1]
                   .Split('"')[0]
                   .Replace("\\", "");
        return token;
    }

    private void GetDefaultHeaders(string token, HttpRequestMessage request)
    {
        GetAppHeaders(request);
        request.Headers.Add("Authority", "www.threads.net");
        request.Headers.Add("Cache-Control", "no-cache");
        request.Headers.Add("Origin", "https://www.threads.net");
        request.Headers.Add("x-fb-lsd", token);
    }

    private void GetAppHeaders(HttpRequestMessage request, string authToken = default)
    {
        request.Headers.Clear();
        request.Headers.Add("Accept", "*/*");
        request.Headers.Add("User-Agent", "Barcelona 289.0.0.77.109 Android");
        if (!string.IsNullOrWhiteSpace(authToken))
        {
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", $"IGT:2:{authToken}");
        }
    }

}