using System.Collections.ObjectModel;

namespace Model.Client
{
    public class Room
    {
        public string ID { get; set; } 
        public string Name { get; set; } 
        public string AvatarUrl { get; set; } 
        public string MessagePreview { get; set; } 
        public string NewMessage { get; set; }

        public ObservableCollection<Message> Messages { get; set; }
    }
}
