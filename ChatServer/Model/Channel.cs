using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

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
