using System.Collections.Generic;
using RestSharp.Deserializers;

namespace Model.Server
{
    public class SyncState
    {
        [DeserializeAs(Name = "next_batch")]
        public string NextBatch { get; set; }

        [DeserializeAs(Name = "rooms.join")]
        public Dictionary<string, Room> JoinedRooms { get; set; }
    }
}
