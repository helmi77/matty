using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Synapse;
using UI.Views;

namespace UI.Pages
{
    public sealed partial class Login : Page
    {
        public LoginViewModel ViewModel { get; set; }

        public event EventHandler LoggedIn;

        public Login()
        {
            InitializeComponent();
            ViewModel = new LoginViewModel();
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            await LoginAsync();
        }

        private async void Login_KeyPressed(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
                await LoginAsync();
        }

        private async Task LoginAsync()
        {
            await SynapseClient.Instance.LoginAsync(ViewModel.Username, ViewModel.Password, ViewModel.HomeserverUrl);
            LoggedIn?.Invoke(this, EventArgs.Empty);
        }
    }
}
