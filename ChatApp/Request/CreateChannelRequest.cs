﻿namespace ChatApp.Request
{
    public class CreateChannelRequest
    {
        public string ChannelName { get; set; }
        public int TeamId { get; set; }
        public int UserId { get; set; }
    }
}
