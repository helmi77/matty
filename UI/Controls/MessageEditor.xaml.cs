using System;
using System.Diagnostics;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Synapse;

namespace UI.Controls
{
    public sealed partial class MessageEditor : UserControl
    {
        public string Message
        {
            get => (string)GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }

        public event EventHandler<RoutedEventArgs> SendClicked;
        public event EventHandler<KeyRoutedEventArgs> EnterPressed;

        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register(
                "Message", typeof(string), typeof(MessageEditor),
                new PropertyMetadata(default(string), MessagePropertyChanged));

        private bool lockChangeExecution;

        public MessageEditor()
        {
            InitializeComponent();
        }

        private void Message_TextChanged(object sender, RoutedEventArgs e)
        {
            if (!lockChangeExecution)
            {
                lockChangeExecution = true;
                Editor.Document.GetText(TextGetOptions.None, out string text);
                if (string.IsNullOrWhiteSpace(text))
                {
                    Message = "";
                }
                else
                {
                    Editor.Document.GetText(TextGetOptions.None, out text);
                    Message = text;
                }
                lockChangeExecution = false;
            }
        }

        private static void MessagePropertyChanged(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var rtb = dependencyObject as MessageEditor;
            if (rtb == null) return;
            if (!rtb.lockChangeExecution)
            {
                rtb.lockChangeExecution = true;
                rtb.Editor.Document.SetText(TextSetOptions.None, rtb.Message);
                rtb.lockChangeExecution = false;
            }
        }

        private void SendButton_OnClick(object sender, RoutedEventArgs e)
        {
            SendClicked?.Invoke(this, e);
        }

        private void Editor_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            bool shiftDown = CoreWindow.GetForCurrentThread()
                .GetKeyState(VirtualKey.Shift)
                .HasFlag(CoreVirtualKeyStates.Down);

            if (e.Key == VirtualKey.Enter)
            {
                if (shiftDown)
                {
                    Message += "\n";
                    Editor.Document.Selection.SetRange(Message.Length, Message.Length);
                }
                else
                    EnterPressed?.Invoke(sender, e);

                e.Handled = true;
            }
        }

        private async void SendFile_OnClick(object sender, RoutedEventArgs e)
        {
            FileOpenPicker picker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary,
            };
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".png");
            picker.FileTypeFilter.Add(".gif");

            StorageFile file = await picker.PickSingleFileAsync();
            if (file == null) return;

            var contentUri = await SynapseClient.Instance.UploadFile(file);
            Debug.WriteLine("Uploaded file to " + contentUri);
        }
    }
}
