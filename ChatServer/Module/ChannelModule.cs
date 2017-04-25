using Nancy;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChatServer.Module
{
    public class ChannelModule : NancyModule
    {
        public ChannelModule() : base("/chat")
        {
            Get("/", _ => "This is chat module!!!!");
            //saves message
            Post("/send_message", SendMessage);
            //loads conversation
            Post("/load_messages", GetMessages);
            //checks if there are new messages
            Post("/new_messages", CheckNewMessages);
            //creates a new channel
            Post("/create_channel", CreateNewChannel);
        }

        private async Task<dynamic> CreateNewChannel(dynamic parameters, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private async Task<dynamic> CheckNewMessages(dynamic parameters, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private async Task<dynamic> GetMessages(dynamic parameters, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private async Task<dynamic> SendMessage(dynamic parameters, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
