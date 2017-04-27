using ChatServer.Request;
using Nancy;
using Nancy.ModelBinding;
using System;
using System.Threading;
using System.Threading.Tasks;
using ChatServer.Model;
using System.Linq;

namespace ChatServer.Module
{
    public class ChannelModule : NancyModule
    {
        private GlobalConfig config = new GlobalConfig();

        public ChannelModule() : base("/chat")
        {
            Get("/", _ => "This is chat module!!!!");
            //saves message
            Post("/send_message", SendMessageAsync);
            //loads conversation
            Post("/load_messages", GetMessagesAsync);
            //checks if there are new messages
            Post("/new_messages", CheckNewMessagesAsync);
            //creates a new channel
            Post("/create_channel", CreateNewChannelAsync);
        }

        private async Task<dynamic> CreateNewChannelAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            var request = this.Bind<CreateChannelRequest>();

            if (!checkToken(request.token))
            {
                return "Log in please";
            }

            try
            {
                if(channelExists(request.ChannelName, request.TeamId))
                {
                    return "Channel with that name already exists";
                } 
                else
                {
                    createChannel(request.ChannelName, request.TeamId);
                }
            } 
            catch(Exception e)
            {
                throw e;
            }

            return "Channel successfully created!";
        }

        private void createChannel(string channelName, int teamId)
        {
            using (var context = new ChatContext(config))
            {
                var channel = new Channel();

                channel.TeamId = teamId;
                channel.ChannelName = channelName;

                context.Channels.Add(channel);

                context.SaveChanges();
            } 
        }

        private bool channelExists(string channelName, int teamId)
        {
            using (var context = new ChatContext(config))
            {
                return context.Channels.Any(c => c.ChannelName == channelName && c.TeamId == teamId);
            }
        }

        private bool checkToken(Token token)
        {
            throw new NotImplementedException();
        }

        private async Task<dynamic> CheckNewMessagesAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private async Task<dynamic> GetMessagesAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private async Task<dynamic> SendMessageAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
