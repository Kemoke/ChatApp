using ChatServer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatServer.Request
{
    class GetMessagesRequest
    {
        public int channelId { get; set; }
        public int senderId { get; set; }
        public int targetId { get; set; }
        public Token token { get; set; }
    }
}
