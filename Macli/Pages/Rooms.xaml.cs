using Macli.Synapse;
using Macli.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Macli.Views.Models;
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

            ThreadPoolTimer.CreatePeriodicTimer(RefreshTimerElapsedHandler, TimeSpan.FromSeconds(1));
        }

        private async void RefreshTimerElapsedHandler(ThreadPoolTimer timer)
        {
            var rooms = await SynapseClient.Instance.SynchronizeAsync();
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                foreach (var room in rooms)
                {
                    var matching = ViewModel.Rooms.FirstOrDefault(r => r.ID == room.ID);
                    if (matching != null)
                    {
                        foreach (var message in room.Messages)
                        {
                            matching.Messages.Add(message);
                        }
                    }
                    else
                    {
                        ViewModel.Rooms.Add(room);
                    }
                }
            });
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