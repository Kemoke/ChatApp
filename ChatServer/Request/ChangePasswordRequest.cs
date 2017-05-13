using ChatServer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatServer.Request
{
    public class ChangePasswordRequest : BaseRequest
    {
        public int UserId { get; set; }
        public string NewPassword { get; set; }
        public string OldPassword { get; set; }
    }
}
