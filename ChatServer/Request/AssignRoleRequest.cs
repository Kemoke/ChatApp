using ChatServer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatServer.Request
{
    public class AssignRoleRequest : BaseRequest
    {
        public int RoleId { get; set; }
        public int UserId { get; set; }
        public int TeamId { get; set; }
    }
}
