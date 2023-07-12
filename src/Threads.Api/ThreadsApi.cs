using Microsoft.AspNetCore.WebUtilities;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Threads.Api.Exceptions;
using Threads.Api.Models;
using Threads.Api.Models.Response;

namespace Threads.Api;

public partial class ThreadsApi : IThreadsApi
{
    private readonly HttpClient _client;
    private const string _url = "https://www.threads.net/";
    private const string _graphUrl = "https://www.threads.net/api/graphql";
    private const string BaseApiUrl = "https://i.instagram.com/api/v1";
    private const string _deviceId = "android-1vp2aultsmo00000";

    private readonly string LoginUrl = $"{BaseApiUrl}/bloks/apps/com.bloks.www.bloks.caa.login.async.send_login_request/";
    private readonly string PostUrl = $"{BaseApiUrl}/media/configure_text_only_post/";

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
        GetDefaultHeaders(null, request);

        request.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
        request.Headers.Add("Accept-Language", "ko,en;q=0.9,ko-KR;q=0.8,ja;q=0.7");
        request.Headers.Add("Pragma", "no-cache");
        request.Headers.Add("Referer", $"https://www.instagram.com/@${username}");
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
    public async Task<LoginResult> LoginAsync(string username, string password, CancellationToken cancellationToken = default)
    {
        var encryptedPassword = await EncryptPasswordAsync(password);

        var requestBody = new
        {
            client_input_params = new
            {
                password = $"#PWD_INSTAGRAM:4:{encryptedPassword.Time}:{encryptedPassword.Password}",
                contact_point = username,
                device_id = _deviceId
            },
            server_params = new
            {
                credential_type = "password",
                device_id = _deviceId
            }
        };

        var json = JsonSerializer.Serialize(requestBody);
        var param = Uri.EscapeDataString(json);

        var blockVersion = "5f56efad68e1edec7801f630b5c122704ec5378adbee6609a448f105f34a9c73";

        var bkClientContext = JsonSerializer.Serialize(new
        {
            bloks_version = blockVersion,
            styles_id = "instagram"
        });

        var request = new HttpRequestMessage(HttpMethod.Post, new Uri(LoginUrl));
        var content = new StringContent($"params={param}&bk_client_context={Uri.EscapeDataString(bkClientContext)}&bloks_versioning_id={blockVersion}");

        GetAppHeaders(request);
        request.Content = content;
        request.Content.Headers.Clear();
        request.Content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
        request.Content.Headers.Add("Response-Type", "text");

        var response = await _client.SendAsync(request, cancellationToken).ConfigureAwait(false);
        var data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);


        var token = data.Split("Bearer IGT:2:")[1].Split("\"")[0].Replace("\\", "");
        var userID = Regex.Match(data, @"pk_id"":""(\d+)").Groups[1].Value;


        return new LoginResult { Token = token, UserId = userID };
    }


    static private readonly SecureRandom secureRandom = new SecureRandom();
    private async Task<(long Time, string Password)> EncryptPasswordAsync(string password)
    {
        // https://github.com/ramtinak/InstagramApiSharp/blob/master/src/InstagramApiSharp/API/InstaApi.cs#L959
        var keys = await SyncLoginExperimentsAsync().ConfigureAwait(false);
        var pubKey = keys.PublicKey;
        var pubKeyId = keys.KeyId;
        byte[] randKey = new byte[32];
        byte[] iv = new byte[12];
        secureRandom.NextBytes(randKey, 0, randKey.Length);
        secureRandom.NextBytes(iv, 0, iv.Length);
        long time = ToUnixTime(DateTime.UtcNow);
        byte[] associatedData = Encoding.UTF8.GetBytes(time.ToString());
        var pubKEY = Encoding.UTF8.GetString(Convert.FromBase64String(pubKey));
        byte[] encryptedKey;
        using (var rdr = PemKeyUtils.GetRSAProviderFromPemString(pubKEY.Trim()))
            encryptedKey = rdr.Encrypt(randKey, false);

        byte[] plaintext = Encoding.UTF8.GetBytes(password);

        var cipher = new GcmBlockCipher(new AesEngine());
        var parameters = new AeadParameters(new KeyParameter(randKey), 128, iv, associatedData);
        cipher.Init(true, parameters);

        var ciphertext = new byte[cipher.GetOutputSize(plaintext.Length)];
        var len = cipher.ProcessBytes(plaintext, 0, plaintext.Length, ciphertext, 0);
        cipher.DoFinal(ciphertext, len);

        var con = new byte[plaintext.Length];
        for (int i = 0; i < plaintext.Length; i++)
            con[i] = ciphertext[i];
        ciphertext = con;
        var tag = cipher.GetMac();

        byte[] buffersSize = BitConverter.GetBytes(Convert.ToInt16(encryptedKey.Length));
        byte[] encKeyIdBytes = BitConverter.GetBytes(Convert.ToUInt16(pubKeyId));
        if (BitConverter.IsLittleEndian)
            Array.Reverse(encKeyIdBytes);
        encKeyIdBytes[0] = 1;
        var payload = Convert.ToBase64String(encKeyIdBytes.Concat(iv).Concat(buffersSize).Concat(encryptedKey).Concat(tag).Concat(ciphertext).ToArray());

        return (time, payload);
    }

    private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    public static long ToUnixTime(DateTime date)
    {
        try
        {
            return Convert.ToInt64((date - UnixEpoch).TotalSeconds);
        }
        catch
        {
            return 0;
        }
    }

    private async Task<(string KeyId, string PublicKey)> SyncLoginExperimentsAsync(CancellationToken cancellationToken = default)
    {
        var loginData = new LoginData();
        var signedData = Sign(loginData);
        var request = new HttpRequestMessage(HttpMethod.Post, new Uri($"{BaseApiUrl}/qe/sync/"));
        request.Content = new FormUrlEncodedContent(signedData);
        GetAppHeaders(request);
        request.Headers.Add("Sec-Fetch-Site", "same-origin");
        request.Headers.Add("X-DEVICE-ID", loginData.id.ToString());

        var response = await _client.SendAsync(request, cancellationToken).ConfigureAwait(false);

        response.Headers.TryGetValues("ig-set-password-encryption-key-id", out var keyidHeaders);
        response.Headers.TryGetValues("ig-set-password-encryption-pub-key", out var pubKeyHeaders);

        if (!keyidHeaders.Any() || !pubKeyHeaders.Any())
        {
            throw new InvalidOperationException("Could not log in");
        }

        return (keyidHeaders.First(), pubKeyHeaders.First());
    }

    private Dictionary<string, string> Sign(LoginData data)
    {
        var json = JsonSerializer.Serialize(data);
        var signature = GetHMAC(json, Constants.SignatureKey);

        return new Dictionary<string, string>
        {
            { "ig_sig_key_version", 4.ToString() },
            { "signed_body", $"{signature}.{json}" },
        };
    }

    private string GetHMAC(string text, string key)
    {
        using var hmacsha256 = new HMACSHA256(Encoding.UTF8.GetBytes(key));
        var hash = hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(text));
        return Convert.ToBase64String(hash);
    }

    private class LoginData
    {
        public Guid id { get; set; } = Guid.NewGuid();
        public string experiments { get; set; } = Constants.LoginExperiments;
    }

    /// <inheritdoc/>
    public async Task<string> PostAsync(string username, string message, string authToken, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(username))
        {
            throw new ArgumentNullException(username);
        }
        if (string.IsNullOrWhiteSpace(message))
        {
            throw new ArgumentNullException(message);
        }
        if (string.IsNullOrWhiteSpace(authToken))
        {
            throw new ArgumentNullException(authToken);
        }

        var now = DateTime.Now;
        var userId = await GetUserIdFromUserNameAsync(username, cancellationToken);
        var data = new
        {
            text_post_app_info = new { reply_control = 0 },
            timezone_offset = "3600",
            source_type = '4',
            _uid = userId.UserId,
            device_id = _deviceId,
            caption = message,
            upload_id = now.Millisecond.ToString(),
            device = new
            {
                manufacturer = "OnePlus",
                model = "ONEPLUS+A3010",
                os_version = 25,
                os_release = "7.1.1",
            },
            publish_mode = "text_post"
        };

        // TODO Image
        // TODO Parent Thread/Post

        var payload = $"signed_body=SIGNATURE.{JsonSerializer.Serialize(data)}";

        var request = new HttpRequestMessage(HttpMethod.Post, new Uri(PostUrl))
        {
            Content = new StringContent(payload)
        };
        GetAppHeaders(request, authToken);
        request.Content.Headers.Clear();
        request.Content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
        request.Content.Headers.Add("Response-Type", "text");

        var response = await _client.SendAsync(request, cancellationToken).ConfigureAwait(false);

        var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
        var postData = await JsonSerializer.DeserializeAsync<PostResponse>(stream).ConfigureAwait(false);

        return postData?.media?.id;
    }

    /// <inheritdoc/>
    public Task<bool> FollowAsync(int userId, string token, string authToken, CancellationToken cancellationToken = default)
    {
        var followUrl = $"{BaseApiUrl}/friendships/create/{userId}/";
        return FollowUnfollowInternal(followUrl, userId, token, authToken, cancellationToken);
    }

    /// <inheritdoc/>
    public Task<bool> UnFollowAsync(int userId, string token, string authToken, CancellationToken cancellationToken = default)
    {
        var url = $"{BaseApiUrl}/friendships/destroy/{userId}/";
        return FollowUnfollowInternal(url, userId, token, authToken, cancellationToken);
    }

    private async Task<bool> FollowUnfollowInternal(string url, int userId, string token, string authToken, CancellationToken cancellationToken = default)
    {
        if (userId <= 0)
        {
            throw new ArgumentNullException(nameof(userId));
        }

        var request = new HttpRequestMessage(HttpMethod.Post, url);
        GetDefaultHeaders(token, request, authToken);
        request.Headers.Add("referrer", "https://www.threads.net/@tidusjar");

        var response = await _client.SendAsync(request, cancellationToken).ConfigureAwait(false);

        var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
        var data = await JsonSerializer.DeserializeAsync<GenericResponse>(stream).ConfigureAwait(false);

        return data?.IsSuccess ?? false;
    }

    private void GetDefaultHeaders(string token, HttpRequestMessage request, string authToken = default)
    {
        GetAppHeaders(request, authToken);
        request.Headers.Add("Authority", "www.threads.net");
        request.Headers.Add("Cache-Control", "no-cache");
        request.Headers.Add("Origin", "https://www.threads.net");
        request.Headers.Add("x-fb-lsd", token);
        request.Headers.Add("x-asbd-id", "129477");
        request.Headers.Add("x-ig-app-id", "238260118697367");
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