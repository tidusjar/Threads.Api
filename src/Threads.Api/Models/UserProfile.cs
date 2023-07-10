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
        public bool IsPrivate { get; set; }
        [JsonPropertyName("profile_pic_url")]
        public string ProfilePicUrl { get; set; }
        [JsonPropertyName("username")]
        public string UserName { get; set; }
        [JsonPropertyName("hd_profile_pic_versions")]
        public Hd_Profile_Pic_Versions[] HDProfilePicVersions { get; set; }
        [JsonPropertyName("is_verified")]
        public bool IsVerified { get; set; }
        [JsonPropertyName("biography")]
        public string Biography { get; set; }
        [JsonPropertyName("biography_with_entities")]
        public object BiographyWithEntities { get; set; }
        [JsonPropertyName("follower_count")]
        public int FollowerCount { get; set; }
        //[JsonPropertyName("profile_context_facepile_users")]
        //public object profile_context_facepile_users { get; set; }
        [JsonPropertyName("bio_links")]
        public Bio_Links[] BioLinks { get; set; }
        [JsonPropertyName("pk")]
        public string PrimaryKey { get; set; }
        [JsonPropertyName("full_name")]
        public string? FullName { get; set; }
        [JsonPropertyName("id")]
        public object Id { get; set; }
    }

    public class Hd_Profile_Pic_Versions
    {
        public int height { get; set; }
        public string url { get; set; }
        public int width { get; set; }
    }

    public class Bio_Links
    {
        public string url { get; set; }
    }

    public class Extensions
    {
        public bool is_final { get; set; }
    }



   
}
