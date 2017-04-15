using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ChatServer.Model
{
    class Team
    {
        [Key]
        public int teamId { get; set; }
        public string name { get; set; }
    }
}
