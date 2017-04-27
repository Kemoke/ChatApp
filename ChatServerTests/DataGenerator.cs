using System.Collections.Generic;
using System.Linq;
using Bogus;
using ChatServer;
using ChatServer.Model;

namespace ChatServerTests
{
    public static class DataGenerator {
        public static Channel GenerateSingleChannel(ChatContext context)
        {
            var channelGenerator = new Faker<Channel>()
                .StrictMode(true)
                .RuleFor(c => c.TeamId, t => t.PickRandom(context.Teams).Id)
                .RuleFor(c => c.ChannelName, t => t.Name.JobTitle());
            return channelGenerator.Generate();
        }

        public static IEnumerable<Channel> GenerateChannelList(ChatContext context, int count)
        {
            var channelGenerator = new Faker<Channel>()
                .StrictMode(true)
                .RuleFor(c => c.TeamId, t => t.PickRandom(context.Teams).Id)
                .RuleFor(c => c.ChannelName, t => t.Name.JobTitle());
            return channelGenerator.Generate(count);
        }
    }
}