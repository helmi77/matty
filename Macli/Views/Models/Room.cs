using System.Collections.ObjectModel;

namespace Macli.Views.Models
{
    public class Room : BindableBase
    {
        private string id;
        private string name;
        private string avatarUrl;
        private string messagePreview;
        private string newMessage;

        private ObservableCollection<Message> messages;

        public string ID
        {
            get => id;
            set => SetProperty(ref id, value);
        }
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }
        public string AvatarUrl
        {
            get => avatarUrl;
            set => SetProperty(ref avatarUrl, value);
        }
        public string MessagePreview
        {
            get => messagePreview;
            set => SetProperty(ref messagePreview, value);
        }
        public string NewMessage
        {
            get => newMessage;
            set => SetProperty(ref newMessage, value);
        }

        public ObservableCollection<Message> Messages
        {
            get => messages;
            set => SetProperty(ref messages, value);
        }
    }
}
