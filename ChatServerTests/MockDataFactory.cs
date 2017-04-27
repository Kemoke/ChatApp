using System;
using System.Collections.Generic;
using ChatServer;
using ChatServer.Model;

namespace ChatServerTests
{
    public class MockDataFactory
    {
        public static GlobalConfig DbConfig { get; set; }
        private static readonly List<User> userData = new List<User>
        {
            new User
            {
                Username = "Kemoke",
                Email = "mail@kemoke.net",
                Password = "pazzw0rd",
                DateOfBirth = DateTime.Now,
                Gender = "Male",
                Company = "Kemokesoft",
                Country = "Bosnia",
                FirstName = "Kemal",
                LastName = "Hrelja"
            }
        };
        private static readonly List<Team> teamData = new List<Team>
        {
            new Team
            {
                Name = "Team1"
            }
        };
        private static readonly List<Channel> channelData = new List<Channel>
        {
            new Channel
            {
                ChannelName = "Channel1",
                TeamId = 0
            }
        };

        public static void PopulateDatabase()
        {
            using (var context = new ChatContext(DbConfig))
            {
                context.Users.AddRange(userData);
                context.Teams.AddRange(teamData);
                context.Channels.AddRange(channelData);
                context.SaveChanges();
            }
        }
    }
}