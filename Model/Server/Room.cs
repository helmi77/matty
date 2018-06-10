using RestSharp.Deserializers;

namespace Model.Server
{
    public class Room
    {
        [DeserializeAs(Name = "timeline")]
        public RoomHistory History { get; set; }

        [DeserializeAs(Name = "state")]
        public RoomState State { get; set; }
    }
}