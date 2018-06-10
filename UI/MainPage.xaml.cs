using Windows.UI.Xaml.Controls;
using Storage;
using Synapse;
using UI.Pages;

namespace UI
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();

            if (SynapseClient.Instance.User != null)
                Root.Navigate(typeof(Overview));
        }

        private void LoginPanel_LoggedIn(object sender, System.EventArgs e)
        {
            AppStorage.SaveUser(SynapseClient.Instance.User);
            Root.Navigate(typeof(Overview));
        }
    }
}
