using System;
using System.Collections.ObjectModel;
using Windows.System;
using Windows.UI.Xaml.Controls;
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
            {
                // TODO: Lazy load user profiles
                ViewModel.Rooms.Add(room);
            }
        }

        private async void Markdown_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            if (Uri.TryCreate(e.Link, UriKind.Absolute, out Uri link))
            {
                await Launcher.LaunchUriAsync(link);
            }
        }
    }
}