using RestSharp.Deserializers;

namespace Model.Server
{
    public class UserProfile
    {
        [DeserializeAs(Name = "avatar_url")]
        public string AvatarUrl { get; set; }

        [DeserializeAs(Name = "displayname")]
        public string DisplayName { get; set; }
    }
}
