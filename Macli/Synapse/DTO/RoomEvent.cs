using RestSharp.Deserializers;

namespace Macli.Synapse.DTO
{
    class RoomEvent
    {
        [DeserializeAs(Name = "origin_server_ts")]
        public string Timestamp { get; set; }

        public string Sender { get; set; }

        [DeserializeAs(Name = "event_id")]
        public string EventID { get; set; }

        public EventContent Content { get; set; }

        [DeserializeAs(Name = "type")]
        public string Type { get; set; }

        public string Membership { get; set; }
    }
}