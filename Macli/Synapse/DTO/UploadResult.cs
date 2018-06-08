using RestSharp.Deserializers;

namespace Macli.Synapse.DTO
{
    public class UploadResult
    {
        [DeserializeAs(Name = "content_uri")]
        public string ContentUri { get; set; }
    }
}
