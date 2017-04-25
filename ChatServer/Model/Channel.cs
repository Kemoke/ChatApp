using System.Collections.Generic;

namespace ChatServer.Model
{
    public class Channel : Entity
    {
        public string ChannelName { get; set; }
        public int TeamId { get; set; }

        public virtual Team Team { get; set; }
        public virtual List<Message> Messages { get; set; }
    }
}
