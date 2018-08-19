using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Model.Client;
using Synapse;
using UI.Views;
using Windows.UI.Xaml.Navigation;
using Storage;

namespace UI.Pages
{
    public sealed partial class Rooms
    {
        public RoomViewModel ViewModel { get; set; }

        private ScrollViewer scroller;
        private ThreadPoolTimer refreshTimer;

        public Rooms()
        {
            InitializeComponent();
            ViewModel = new RoomViewModel();
        }
 
        private async void ScrollToBottom()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                scroller?.ChangeView(0, scroller.ScrollableHeight, 1);
            });
        }

        private void Page_OnUnloaded(object sender, RoutedEventArgs e)
        {
            refreshTimer?.Cancel();
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
                        foreach (var message in room.Messages)
                        {
                            matching.Messages.Add(message);
                            if (ViewModel.SelectedRoom.ID == matching.ID)
                                scroller?.ChangeView(0, scroller.ScrollableHeight, 1);
                        }
                    else
                        ViewModel.Rooms.Add(room); 
                }
            });
        }

        private async void Editor_EnterPressed(object sender, KeyRoutedEventArgs e)
        {
            await SendMessage();
        }

        private async void SendButton_OnClick(object sender, RoutedEventArgs e)
        {
            await SendMessage();
        }

        private async Task SendMessage()
        {
            await SynapseClient.Instance.SendMessageAsync(ViewModel.SelectedRoom);
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.Rooms = new ObservableCollection<Room>();
            var rooms = await AppStorage.LoadRooms();
            if (rooms.Count() <= 0)
            {
                rooms = await SynapseClient.Instance.SynchronizeAsync();
                AppStorage.SaveRooms(rooms);
            }

            foreach (var room in rooms)
                ViewModel.Rooms.Add(room);

            if (ViewModel.Rooms.Count > 0)
                ViewModel.SelectedRoom = ViewModel.Rooms.First();

            ThreadPoolTimer.CreateTimer(timer => ScrollToBottom(), TimeSpan.FromSeconds(0.5));
            refreshTimer = ThreadPoolTimer.CreatePeriodicTimer(RefreshTimerElapsedHandler, TimeSpan.FromSeconds(5));
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            refreshTimer?.Cancel();
            base.OnNavigatedFrom(e);
        }

        private void Scroller_Loaded(object sender, RoutedEventArgs e) => scroller = sender as ScrollViewer; 
        private void Room_OnSelectionChanged(object sender, SelectionChangedEventArgs e) => ScrollToBottom();
    }
}