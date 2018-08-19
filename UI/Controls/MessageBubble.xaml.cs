using System;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Model.Client;
using Synapse;

namespace UI.Controls
{
    public sealed partial class MessageBubble : UserControl
    {
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(Message), typeof(MessageBubble),
                new PropertyMetadata(default(Message), MessagePropertyChanged));

        public static readonly DependencyProperty CaptionProperty = DependencyProperty.Register(
            "Caption", typeof(string), typeof(MessageBubble), new PropertyMetadata(default(string)));

        public Message Message
        {
            get => (Message) GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }

        public string Caption
        {
            get => (string) GetValue(CaptionProperty);
            set => SetValue(CaptionProperty, value);
        }

        private bool lockChangeExecution;

        public MessageBubble()
        {
            InitializeComponent();
        }

        private async void Markdown_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            if (Uri.TryCreate(e.Link, UriKind.Absolute, out Uri link))
            {
                await Launcher.LaunchUriAsync(link);
            }
        }

        private static async void MessagePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MessageBubble messageBubble = d as MessageBubble;
            if (messageBubble == null) return;
            
            Message message = messageBubble.Message;
            if (!messageBubble.lockChangeExecution)
            {
                messageBubble.lockChangeExecution = true;

                if (message.MessageType == "m.image" && !message.Text.Contains("!["))
                {
                    string url = SynapseClient.Instance.GetPreviewUrl(message.Image);
                    message.Text = $"![{message.Text}]({url})";
                }

                if (message.IsLastFollowup)
                {
                    messageBubble.Caption = await ConstructCaption(message);
                }

                messageBubble.lockChangeExecution = false;
            }
        }

        private static async Task<string> ConstructCaption(Message message)
        {
            string caption = string.Empty;
            if (!message.IsMine)
            {
                string displayName = await SynapseClient.Instance.GetDisplayName(message.Sender);
                caption += $"{displayName} • ";
            }

            caption += message.Age;
            return caption;
        }

        private async void Markdown_ImageClicked(object sender, LinkClickedEventArgs e)
        {
            if (Uri.TryCreate(e.Link, UriKind.Absolute, out Uri link))
            {
                await Launcher.LaunchUriAsync(link);
            }
        }
    }
}