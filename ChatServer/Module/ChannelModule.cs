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
