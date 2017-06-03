using System.Collections.Generic;
using System.Linq;
using Bogus;
using ChatServer;
using ChatServer.Model;

namespace ChatServerTests
{
    public static class DataGenerator {
        public static Channel GenerateSingleChannel(ChatContext context, int teamId)
        {
            return GenerateChannelList(context, teamId, 1).First();
        }

        public static IEnumerable<Channel> GenerateChannelList(ChatContext context, int teamId, int count)
        {
            var channelGenerator = new Faker<Channel>()
                .RuleFor(c => c.TeamId, teamId)
                .RuleFor(c => c.ChannelName, t => t.Name.JobTitle());
            return channelGenerator.Generate(count);
        }

        public static IEnumerable<User> GenerateUserList(ChatContext context, int count)
        {
            var userGenerator = new Faker<User>()
                .RuleFor(u => u.FirstName, t => t.Name.FirstName())
                .RuleFor(u => u.LastName, t => t.Name.LastName())
                .RuleFor(u => u.Company, t => t.Company.CompanyName())
                .RuleFor(u => u.Country, t => t.Locale)
                .RuleFor(u => u.DateOfBirth, t => t.Person.DateOfBirth)
                .RuleFor(u => u.Gender, () => "Male")
                .RuleFor(u => u.Password, t => t.Internet.Password())
                .RuleFor(u => u.Username, t => t.Internet.UserName())
                .RuleFor(u => u.Email, t => t.Internet.Email())
                .RuleFor(u => u.PictureUrl, () => "");
            return userGenerator.Generate(count);
        }

        public static IEnumerable<Team> GenerateTeamList(ChatContext context, int count)
        {
            var channelGenerator = new Faker<Team>()
                .RuleFor(t => t.Name, t => t.Name.JobTitle());
                //.RuleFor(t => t.UserId, t => t.)
            return channelGenerator.Generate(count);
        }

        public static User GenerateSingleUser(ChatContext context)
        {
            return GenerateUserList(context, 1).First();
        }

        public static Team GenerateSingleTeam(ChatContext context)
        {
            return GenerateTeamList(context, 1).First();
        }

        public static IEnumerable<Message> GenerateMessageList(ChatContext context, int count, int channelId, int senderId, int targetId)
        {
            var messageGenerator = new Faker<Message>()
                .RuleFor(m => m.MessageText, t => t.Hacker.Phrase())
                .RuleFor(m => m.ChannelId, channelId)
                .RuleFor(m => m.SenderId, senderId)
                .RuleFor(m => m.TargetId, targetId);
            
            return messageGenerator.Generate(count);
        }
    }
}