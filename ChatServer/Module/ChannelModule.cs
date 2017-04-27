using ChatServer.Request;
using Nancy;
using Nancy.ModelBinding;
using System;
using System.Threading;
using System.Threading.Tasks;
using ChatServer.Model;
using System.Linq;
using ChatServer.Response;
using Microsoft.EntityFrameworkCore;

namespace ChatServer.Module
{
    public class ChannelModule : NancyModule
    {
        private readonly GlobalConfig config;
        private readonly ChatContext context;

        public ChannelModule(GlobalConfig config) : base("/chat")
        {
            //ovako uzimaj config i context
            this.config = config;
            context = new ChatContext(config);
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
                return Response.AsJson(new Error("Log in please"));
            }
            //sve sto radi sa bazom koristi async metode i await ispred
            if(await ChannelExistsAsync(request.ChannelName, request.TeamId))
            {
                return Response.AsJson(new Error("Channel with that name already exists"));
            }
            var added = new Channel
            {
                TeamId = request.TeamId,
                ChannelName = request.ChannelName
            };
            context.Channels.Add(added);
            await context.SaveChangesAsync(cancellationToken);
            //uvjek vracaj objekt koji napravis preko apija nazad
            return Response.AsJson(added);
        }

        private async Task<bool> ChannelExistsAsync(string channelName, int teamId)
        {
                return await context.Channels.AnyAsync(c => c.ChannelName == channelName && c.TeamId == teamId);
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
