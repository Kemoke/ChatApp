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
            Get("/load_messages/{skip}/{limit}", GetMessagesAsync);
            //checks if there are new messages
            Post("/new_messages", CheckNewMessagesAsync);
            //creates a new channel
            Post("/create_channel", CreateNewChannelAsync);
        }

        private async Task<dynamic> CreateNewChannelAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            var request = this.Bind<CreateChannelRequest>();

            if (await CheckTokenAsync(request.token))
            {
                return Response.AsJson(new Error("Log in please"));
            }
            //sve sto radi sa bazom koristi async metode i await ispred
            if(await ChannelExistsAsync(request.ChannelName, request.TeamId))
            {
                return Response.AsJson(new Error("Channel with that name already exists"));
            }

            var channel = new Channel
            {
                TeamId = request.TeamId,
                ChannelName = request.ChannelName
            };

            context.Channels.Add(channel);
            await context.SaveChangesAsync(cancellationToken);
            //uvjek vracaj objekt koji napravis preko apija nazad
            return Response.AsJson(channel);
        }

        private async Task<bool> ChannelExistsAsync(string channelName, int teamId)
        {
                return await context.Channels.AnyAsync(c => c.ChannelName == channelName && c.TeamId == teamId);
        }

        private Task<bool> CheckTokenAsync(Token token)
        {
            throw new NotImplementedException();
        }

        private async Task<dynamic> CheckNewMessagesAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private async Task<dynamic> GetMessagesAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            var request = this.Bind<GetMessagesRequest>();

            if (await CheckTokenAsync(request.token))
            {
                return Response.AsJson(new Error("Log in please"));
            }

            var messages = context.Messages.Where(m => m.SenderId == request.senderId && m.TargetId == request.targetId && m.ChannelId == request.channelId).Skip((int)parameters.skip).Take((int)parameters.limit).ToListAsync(cancellationToken);

            
            return Response.AsJson(messages);

        }

        private async Task<dynamic> SendMessageAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
