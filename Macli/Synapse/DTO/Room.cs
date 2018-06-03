using RestSharp.Deserializers;

namespace Macli.Synapse.DTO
{
    class Room
    {
        [DeserializeAs(Name = "timeline")]
        public RoomHistory History { get; set; }

        [DeserializeAs(Name = "state")]
        public RoomState State { get; set; }
    }
}