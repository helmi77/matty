using RestSharp.Deserializers;

namespace Macli.Synapse.DTO
{
    public class EventContent
    {
        public string Body { get; set; }

        [DeserializeAs(Name = "msgtype")]
        public string Type { get; set; }

        [DeserializeAs(Name = "join_rule")]
        public string JoinRule { get; set; }

        public string Membership { get; set; }

        [DeserializeAs(Name = "avatar_url")]
        public string UserAvatarUrl { get; set; }

        [DeserializeAs(Name = "url")]
        public string Url { get; set; }
        [DeserializeAs(Name = "info.thumbnail_url")]
        public string ThumbnailUrl { get; set; }
        
        [DeserializeAs(Name = "info.w")]
        public int Width { get; set; }
        [DeserializeAs(Name = "info.h")]
        public int Height { get; set; }

        public string DisplayName { get; set; }
        public string Name { get; set; }
    }
}