using System.Collections.Generic;
using Macli.Synapse.DTO;

namespace Macli.Processing.Comparers
{
    public class FollowupComparer : EqualityComparer<RoomEvent>
    {
        public override bool Equals(RoomEvent x, RoomEvent y)
        {
            if (x == y) return true;
            if (x == null || y == null) return false;
            return x.Sender.Equals(y.Sender);
        }

        public override int GetHashCode(RoomEvent obj)
        {
            return obj.GetHashCode();
        }
    }
}
