using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Macli.Synapse;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Macli.Controls
{
    public sealed partial class RoomAvatar : UserControl
    {
        public string AvatarUrl
        {
            get => (string) GetValue(AvatarUrlProperty);
            set
            {
                if (!string.IsNullOrEmpty(value))
                    SetValue(AvatarUrlProperty, SynapseClient.Instance.GetPreviewUrl(value, 50, 50));
            }
        }

        public static readonly DependencyProperty AvatarUrlProperty =
            DependencyProperty.Register("AvatarUrl", typeof(string), typeof(RoomAvatar), null);

        public RoomAvatar()
        {
            InitializeComponent();
        }
    }
}
