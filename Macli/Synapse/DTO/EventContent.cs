using RestSharp.Deserializers;

namespace Macli.Synapse.DTO
{
    class EventContent
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
        public string RoomAvatarUrl { get; set; }

        public string DisplayName { get; set; }
        public string Name { get; set; }
    }
}