using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace ChatServer.Model
{
    public class Team : Entity
    {
        public string Name { get; set; }     

        public virtual List<Channel> Channels { get; set; }

        [JsonIgnore]
        public virtual List<UserTeam> UserTeams { get; set; }

        public List<User> Users => UserTeams?.Select(ut => ut.User).ToList();
    }
}
