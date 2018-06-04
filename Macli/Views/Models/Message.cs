using System;
using Humanizer;
using Macli.Synapse;

namespace Macli.Views.Models
{
    public class Message
    {
        public string Text { get; set; }
        public string Sender { get; set; }
        public bool IsMine => Sender.Equals(SynapseClient.Instance.User.ID);
        public string AvatarUrl { get; set; }
        public DateTime Timestamp { get; set; }
        public string Preview { get; set; }

        public string Caption
        {
            get
            {
                var sender = Sender.Substring(1, Sender.IndexOf(':') - 1);
                var caption = string.Empty;
                if (!IsMine)
                    caption = $"{sender} • ";
                caption += Timestamp.Humanize();
                return caption;
            }
        }
    }
}
