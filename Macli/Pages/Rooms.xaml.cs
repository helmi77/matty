using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Macli.Synapse;
using Macli.Views;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Room = Macli.Views.Models.Room;

namespace Macli.Pages
{
    public sealed partial class Rooms : Page
    {
        public RoomViewModel ViewModel { get; set; }

        public Rooms()
        {
            InitializeComponent();
            ViewModel = new RoomViewModel();
        }

        private async void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.Rooms = new ObservableCollection<Room>();
            var rooms = await SynapseClient.Instance.LoadRoomsAsync(ViewModel);
            foreach (var room in rooms)
                ViewModel.Rooms.Add(room);

            if (ViewModel.Rooms.Count > 0)
                ViewModel.SelectedRoom = ViewModel.Rooms.First();
        }

        private async void Markdown_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            if (Uri.TryCreate(e.Link, UriKind.Absolute, out Uri link))
            {
                await Launcher.LaunchUriAsync(link);
            }
        }

        private void Editor_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            // TODO: Send message on enter pressed
        }

        private async void SendButton_OnClick(object sender, RoutedEventArgs e)
        {
            await SendMessage();
        }

        private async Task SendMessage()
        {
            await SynapseClient.Instance.SendMessageAsync(ViewModel.SelectedRoom);
        }
    }
}