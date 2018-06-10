using RestSharp.Deserializers;

namespace Model.Server
{
    public class User
    {
        [DeserializeAs(Name = "user_id")]
        public string ID { get; set; }
        [DeserializeAs(Name = "home_server")]
        public string Homeserver { get; set; }
        [DeserializeAs(Name = "access_token")]
        public string AccessToken { get; set; }

        [DeserializeAs(Name = "error")]
        public string LoginError { get; set; }
    }
}