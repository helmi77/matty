using System.Collections.ObjectModel;
using Macli.Views.Models;

namespace Macli.Views
{
    public class RoomViewModel : BindableBase
    {
        private Room selectedRoom;
        private ObservableCollection<Room> rooms;

        public Room SelectedRoom
        {
            get => selectedRoom;
            set => SetProperty(ref selectedRoom, value);
        }
        public ObservableCollection<Room> Rooms
        {
            get => rooms;
            set => SetProperty(ref rooms, value);
        }
    }
}
