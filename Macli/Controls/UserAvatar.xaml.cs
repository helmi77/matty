using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Synapse;

namespace UI.Controls
{
    public sealed partial class UserAvatar : UserControl
    {
        public string UserID
        {
            get => (string) GetValue(UserIDProperty);
            set => SetValue(UserIDProperty, value);
        }

        public string AvatarUrl
        {
            get => (string) GetValue(AvatarUrlProperty);
            set => SetValue(AvatarUrlProperty, value);
        }
        
        public static readonly DependencyProperty AvatarUrlProperty = DependencyProperty.Register(
            "AvatarUrl", typeof(string), typeof(UserAvatar), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty UserIDProperty = DependencyProperty.Register("UserID", typeof(string),
            typeof(UserAvatar), new PropertyMetadata(default(string), UserIDChanged));

        public UserAvatar()
        {
            InitializeComponent();
        }

        private static async void UserIDChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UserAvatar avatarControl = d as UserAvatar;
            if (avatarControl == null) return;

            string thumbnailPath = await SynapseClient.Instance.GetUserAvatarThumbnailAsync(avatarControl.UserID);
            avatarControl.AvatarUrl = thumbnailPath;
            /*
            Profile profile = await SynapseClient.Instance.GetUserProfileAsync(avatarControl.UserID);
            if (profile?.AvatarUrl == null) return;

            string avatarUrl = SynapseClient.Instance.GetPreviewUrl(profile.AvatarUrl, 36, 36);
            avatarControl.AvatarUrl = avatarUrl;*/
        }
    }
}
