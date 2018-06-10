using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Synapse;

namespace UI.Controls
{
    public sealed partial class RoomAvatar : UserControl
    {
        public string AvatarUrl
        {
            get => (string) GetValue(AvatarUrlProperty);
            set
            {
                if (!string.IsNullOrEmpty(value))
                    SetValue(AvatarUrlProperty, SynapseClient.Instance.GetPreviewUrl(value, 44, 44));
            }
        }

        public static readonly DependencyProperty AvatarUrlProperty =
            DependencyProperty.Register("Url", typeof(string), typeof(RoomAvatar), null);

        public RoomAvatar()
        {
            InitializeComponent();
        }
    }
}
