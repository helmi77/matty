namespace Macli.Views
{
    public class LoginViewModel : BindableBase
    {
        private string username;
        private string password;
        private string homeserverUrl;
        private string errorMessage;
        private bool customHomeserver;

        public string Username
        {
            get => username;
            set => SetProperty(ref username, value);
        }
        
        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }
        
        public string HomeserverUrl
        {
            get => homeserverUrl;
            set => SetProperty(ref homeserverUrl, value);
        }

        public string ErrorMessage
        {
            get => errorMessage;
            set => SetProperty(ref errorMessage, value);
        }

        public bool CustomHomeserver
        {
            get => customHomeserver;
            set => SetProperty(ref customHomeserver, value);
        }
    }
}
