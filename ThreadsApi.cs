using System;
using System.Text.RegularExpressions;

namespace Threads;
public class ThreadsApi
{
    private string fbLSDToken { get; set; } = "NjppQDEgONsU_1LCzrmp6q";
    private HttpClient _client = new HttpClient();
    public async Task<int> GetUserIdFromUserName(string username)
    {

        var request = new HttpRequestMessage(HttpMethod.Get, $"https://www.threads.net/@${username}");
        GetDefaultHeaders(username, request);

        request.Headers.Add("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
        request.Headers.Add("accept-language", "ko,en;q=0.9,ko-KR;q=0.8,ja;q=0.7");
        request.Headers.Add("pragma", "no-cache");
        request.Headers.Add("referer", $"https://www.threads.net/@${username}");
        request.Headers.Add("sec-fetch-dest", "document");
        request.Headers.Add("sec-fetch-mode", "navigate");
        request.Headers.Add("sec-fetch-site", "cross-site");
        request.Headers.Add("sec-fetch-user", "?1");
        request.Headers.Add("upgrade-insecure-requests", "1");
        request.Headers.Add("x-asbd-id", "");
        request.Headers.Add("x-fb-lsd", "");
        request.Headers.Add("x-ig-app-id", "");

        var response = await _client.SendAsync(request);
        var text = await response.Content.ReadAsStringAsync();

        var regexUserId = new Regex(@"/""props"":{""user_id"":""(\\d+)""},""");
        var regexLsdToken = new Regex("/\"LSD\",\\[\\],{\"token\":\"(\\w+)\"},\\d+\\]/");

        var match = regexUserId.Match(text);

        var userId = regexUserId.Match(text).Value;
        var lsdToken = regexLsdToken.Match(text).Value;
        this.fbLSDToken = lsdToken;

        return int.Parse(userId);
    }

    private void GetDefaultHeaders(string username, HttpRequestMessage request)
    {
        request.Headers.Add("authority", "www.threads.net");
        request.Headers.Add("cache-control", "no-cache");
        request.Headers.Add("origin", "https://www.threads.net");
        request.Headers.Add("x-fb-lsd", this.fbLSDToken);
        request.Headers.Add("accept", "*/*");





    }
}