using System.Text.Json.Serialization;

namespace Threads.Api.Models;

public class ErrorResponse
{
    [JsonPropertyName("message")]
    public string Code { get; set; }
    [JsonPropertyName("error_title")]
    public string Title { get; set; }
    [JsonPropertyName("error_body")]
    public string Message { get; set; }
    [JsonPropertyName("status")]
    public string Status { get; set; }
}