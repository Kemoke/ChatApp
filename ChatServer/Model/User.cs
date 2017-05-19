using System;
using System.Collections.Generic;
using System.Linq;

namespace ChatServer.Model
{
    public class User : Entity
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Company { get; set; }
        public string Country { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PictureUrl { get; set; }
        public string Gender { get; set; }

        public virtual List<UserTeam> UserTeams { get; set; }

        public List<Team> Teams => UserTeams?.Select(ut => ut.Team).ToList();
    }
}
