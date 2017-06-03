using ChatServer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatServer.Request
{
    public class CheckNewMessagesRequest
    {
        public int MessageId { get; set; }
        public int ChannelId { get; set; }
    }
}
