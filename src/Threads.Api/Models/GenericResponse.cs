using System;
using System.Text.Json.Serialization;

namespace Threads.Api.Models;

internal class GenericResponse
{
    [JsonPropertyName("status")]
    public string Status { get; set; }
    internal bool IsSuccess => Status.Equals("OK", StringComparison.InvariantCultureIgnoreCase);
}