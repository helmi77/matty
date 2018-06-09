using System;
using System.Diagnostics;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Macli.Synapse;
using Macli.Views.Models;
using Microsoft.Toolkit.Uwp.UI.Controls;

namespace Macli.Controls
{
    public sealed partial class MessageBubble : UserControl
    {
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(Message), typeof(MessageBubble),
                new PropertyMetadata(default(Message), MessagePropertyChanged));
        
        public Message Message
        {
            get => (Message) GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
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

        private static void MessagePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MessageBubble messageBubble = d as MessageBubble;
            if (messageBubble == null) return;
            
            Message message = messageBubble.Message;
            if (message.MessageType == "m.image")
            {
                string url = SynapseClient.Instance.GetPreviewUrl(message.Image);
                if (!messageBubble.lockChangeExecution)
                {
                    messageBubble.lockChangeExecution = true;
                    message.Text = $"![{message.Text}]({url})";
                    messageBubble.lockChangeExecution = false;
                }
            }
        }

        private void OnImageResolving(object sender, ImageResolvingEventArgs e)
        {
        }
    }
}