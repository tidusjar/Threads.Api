namespace Threads.Api.Models;

public class UserThreads
{
    public UserThreadData data { get; set; }
}

public class UserThreadData
{
    public Mediadata mediaData { get; set; }
}

public class Mediadata
{
    public Thread[] threads { get; set; }
}

public class Thread
{
    public Thread_Items[] thread_items { get; set; }
    public string id { get; set; }
}

public class Thread_Items
{
    public Post post { get; set; }
    public string line_type { get; set; }
    public object view_replies_cta_string { get; set; }
    public object[] reply_facepile_users { get; set; }
    public bool should_show_replies_cta { get; set; }
    public string __typename { get; set; }
}

public class Post
{
    public UserLite user { get; set; }
    public Image_Versions2 image_versions2 { get; set; }
    public int original_width { get; set; }
    public int original_height { get; set; }
    public object[] video_versions { get; set; }
    public object carousel_media { get; set; }
    public object carousel_media_count { get; set; }
    public string pk { get; set; }
    public object has_audio { get; set; }
    public Text_Post_App_Info text_post_app_info { get; set; }
    public Caption caption { get; set; }
    public int taken_at { get; set; }
    public int like_count { get; set; }
    public string code { get; set; }
    public object media_overlay_info { get; set; }
    public string id { get; set; }
}

public class UserLite
{
    public string profile_pic_url { get; set; }
    public string username { get; set; }
    public object id { get; set; }
    public bool is_verified { get; set; }
    public string pk { get; set; }
}

public class Image_Versions2
{
    public object[] candidates { get; set; }
}

public class Text_Post_App_Info
{
    public object link_preview_attachment { get; set; }
    public Share_Info share_info { get; set; }
    public object reply_to_author { get; set; }
    public bool is_post_unavailable { get; set; }
}

public class Share_Info
{
    public object quoted_post { get; set; }
    public object reposted_post { get; set; }
}

public class Caption
{
    public string text { get; set; }
}

