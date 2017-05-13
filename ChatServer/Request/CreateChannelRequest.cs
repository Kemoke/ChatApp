﻿using ChatServer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatServer.Request
{
    public class CreateChannelRequest : BaseRequest
    {
        public string ChannelName { get; set; }
        public int TeamId { get; set; }
    }
}
