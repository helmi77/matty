using System.Collections.Generic;

namespace Macli.Synapse.DTO
{
    class RoomHistory
    {
        public bool Limited { get; set; }
        public string PrevBatch { get; set; }
        public List<RoomEvent> Events { get; set; }
    }
}