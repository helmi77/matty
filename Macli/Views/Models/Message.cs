using System;
using Humanizer;
using Macli.Synapse;

namespace Macli.Views.Models
{
    public class Message
    {
        public string Sender { get; set; }
        public string Text { get; set; }
        public string Url { get; set; }

        public bool IsFollowup { get; set; }
        public bool IsLastFollowup { get; set; }
        public bool IsMine => Sender.Equals(SynapseClient.Instance.User.ID);
        public bool ShowDetails => !IsFollowup;

        public DateTime Timestamp { get; set; }

        public Picture Image { get; set; }

        public string MessageType { get; set; }

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

    public class Picture
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string ThumbnailUrl { get; set; }
    }
}
