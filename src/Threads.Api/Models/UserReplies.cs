namespace Threads.Api.Models;

public class UserReplies
{
    public ReplyData data { get; set; }
}

public class ReplyData
{
    public ReplyMediadata mediaData { get; set; }
}

public class ReplyMediadata
{
    public ReplyThread[] threads { get; set; }
}

public class ReplyThread
{
    public ReplyThread_Items[] thread_items { get; set; }
    public string id { get; set; }
}

public class ReplyThread_Items
{
    public ReplyPost post { get; set; }
    public string line_type { get; set; }
    public string view_replies_cta_string { get; set; }
    public Reply_Facepile_Users[] reply_facepile_users { get; set; }
    public bool should_show_replies_cta { get; set; }
    public string __typename { get; set; }
}

public class ReplyPost
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

public class Reply_To_Author
{
    public string username { get; set; }
    public object id { get; set; }
}

public class Reply_Facepile_Users
{
    public string __typename { get; set; }
    public object id { get; set; }
    public string profile_pic_url { get; set; }
}
