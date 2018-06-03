using Macli.Synapse;

namespace Macli.Views.Models
{
    public class Message
    {
        public string Text { get; set; }
        public string Sender { get; set; }
        public bool IsMine => Sender.Equals(SynapseClient.Instance.User.ID);
        public string AvatarUrl { get; set; }
    }
}
