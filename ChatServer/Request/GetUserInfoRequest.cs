using ChatServer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatServer.Request
{
    public class GetUserInfoRequest
    {
        public int UserId { get; set; }
    }
}
