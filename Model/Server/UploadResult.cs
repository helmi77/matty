using RestSharp.Deserializers;

namespace Model.Server
{
    public class UploadResult
    {
        [DeserializeAs(Name = "content_uri")]
        public string ContentUri { get; set; }
    }
}
