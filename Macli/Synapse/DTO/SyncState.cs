﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Deserializers;

namespace Macli.Synapse.DTO
{
    class SyncState
    {
        [DeserializeAs(Name = "next_batch")]
        public string NextBatch { get; set; }

        [DeserializeAs(Name = "rooms.join")]
        public Dictionary<string, Room> JoinedRooms { get; set; }
    }
}
