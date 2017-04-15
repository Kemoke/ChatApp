using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ChatServer.Model
{
    class UserTeam
    {
        [Key]
        public int userTeamId { get; set; }
        public int userId { get; set; }
        public int teamId { get; set; }
        public int roleId { get; set; }
    }
}
