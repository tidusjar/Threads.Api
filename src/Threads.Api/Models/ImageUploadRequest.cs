namespace Threads.Api.Models;


public class ImageUploadRequest
{
    /// <summary>
    /// Can be either a local path or remote
    /// </summary>
    public string Path { get; set; }
    /// <summary>
    /// Byte[] of the image content
    /// </summary>
    public byte[] Content { get; set; }
    /// <summary>
    /// Use in conjunction with <see cref="Content"/>
    /// </summary>
    public string MimeType { get; set; }
}
