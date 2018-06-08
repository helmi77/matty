using RestSharp.Deserializers;

namespace Macli.Synapse.DTO
{
    public class UserProfile
    {
        [DeserializeAs(Name = "avatar_url")]
        public string AvatarUrl { get; set; }

        [DeserializeAs(Name = "displayname")]
        public string DisplayName { get; set; }
    }
}
