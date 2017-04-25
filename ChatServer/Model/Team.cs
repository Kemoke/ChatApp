using System.Collections.Generic;
using System.Linq;

namespace ChatServer.Model
{
    public class Team : Entity
    {
        public string Name { get; set; }

        public virtual List<Channel> Channels { get; set; }
        public virtual List<UserTeam> UserTeams { get; set; }

        public List<User> Users => UserTeams.Select(ut => ut.User).ToList();

        public void AddUser(User user, Role role)
        {
            var userTeam = new UserTeam
            {
                TeamId = Id,
                UserId = user.Id,
                RoleId = role.Id
            };
        }

        public void RemoveUser(User user, Role role)
        {
            //
        }

        public void EditUserRole(User user, Role role)
        {
            var userTeam = UserTeams.Find(u => u.UserId == user.Id && u.TeamId == Id);
            userTeam.RoleId = role.Id;
        }
    }
}
