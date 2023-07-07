using System.Text.Json.Serialization;

namespace Threads.Api.Models
{
    public class UserProfile
    {
        [JsonPropertyName("data")]
        public Data Data { get; set; }
    }

    public class Data
    {
        [JsonPropertyName("userData")]
        public Userdata UserData { get; set; }
    }

    public class Userdata
    {
        [JsonPropertyName("user")]
        public User User { get; set; }
    }

    public class User
    {
        [JsonPropertyName("is_private")]
        public bool? Private { get; set; }
        [JsonPropertyName("profile_pic_url")]
        public string ProfilePicUrl { get; set; }
        [JsonPropertyName("username")]
        public string Username { get; set; }
        [JsonPropertyName("hd_profile_pic_versions")]
        public string? HdProfilePicVersions { get; set; }
        [JsonPropertyName("is_verified")]
        public bool? IsVerified { get; set; }
        [JsonPropertyName("biography")]
        public string? Bio { get; set; }
        [JsonPropertyName("biography_with_entities")]
        public string? BioWithEntities { get; set; }
        [JsonPropertyName("follower_count")]
        public string? Followers { get; set; }
        //[JsonPropertyName("username")]
        //public object profile_context_facepile_users { get; set; }
        //[JsonPropertyName("bio_links")]
        //public object bio_links { get; set; }
        [JsonPropertyName("pk")]
        public string? UserId { get; set; }
        [JsonPropertyName("full_name")]
        public string? FullName { get; set; }
        //[JsonPropertyName("username")]
        //public object id { get; set; }
    }
}
