using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Macli.Controls
{
    public sealed partial class UserAvatar : UserControl
    {
        public string AvatarUrl
        {
            get => (string)GetValue(AvatarUrlProperty);
            set => SetValue(AvatarUrlProperty, value);
        }

        public static readonly DependencyProperty AvatarUrlProperty =
            DependencyProperty.Register("Url", typeof(string), typeof(UserAvatar), null);

        public UserAvatar()
        {
            InitializeComponent();
        }
    }
}
