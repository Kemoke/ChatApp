using ChatServer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatServer.Request
{
    public class GetMessagesRequest
    {
        public int ChannelId { get; set; }
        public int SenderId { get; set; }
        public int TargetId { get; set; }
    }
}
