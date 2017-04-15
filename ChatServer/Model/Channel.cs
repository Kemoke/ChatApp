using System;
using System.Collections.Generic;
using System.Text;

namespace ChatServer.Model
{
    class Channel
    {
        public int channelId { get; set; }
        public string channelName { get; set; }
        public int teamId { get; set; }
        public int userId { get; set; }
    }
}
