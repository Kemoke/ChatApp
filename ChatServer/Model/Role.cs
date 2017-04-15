using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ChatServer.Model
{
    class Role
    {
        [Key]
        public int roleId { get; set; }
        public string roleName { get; set; }
    }
}
