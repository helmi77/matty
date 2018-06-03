using System.Collections.Generic;
using Macli.Synapse.DTO;

namespace Macli.Views.Models
{
    public class Room
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public ICollection<Message> Messages { get; set; }
        public string AvatarUrl { get; set; }
        public string MessagePreview { get; set; }
    }
}
