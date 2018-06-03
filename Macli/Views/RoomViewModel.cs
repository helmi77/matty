using System.Collections.ObjectModel;
using Macli.Views.Models;

namespace Macli.Views
{
    public class RoomViewModel : BindableBase
    {
        private ObservableCollection<Room> rooms;

        public ObservableCollection<Room> Rooms
        {
            get => rooms;
            set => SetProperty(ref rooms, value);
        }
    }
}
