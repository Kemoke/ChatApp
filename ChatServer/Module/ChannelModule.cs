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
using System.Collections.Generic;

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
            if (!await CheckTokenAsync(request.Token))
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
            //posaljemo mu ID zadnje poruke, provjerimo da li je to zadnja koja je spasena u bazi, ako nije onda vratimo sve ispred nje.

            var request = this.Bind<CheckNewMessagesRequest>();

            if(await CheckTokenAsync(request.Token))
            {
                return Response.AsJson(new Error("Log in please"));

            }

            var messages = new List<Message>();
            
            if (context.Messages.Any())
            {
                var lastId = context.Messages.Last().Id;
                if (lastId == request.MessageId)
                {
                    return Response.AsJson(messages);
                }
            }
            else
            {
                return Response.AsJson(new Error("There are no messages to be dipslayed"));
            }
            

            messages = await context.Messages.Where(m => (m.Id > request.MessageId && m.Id == request.ChannelId)).ToListAsync();

            return Response.AsJson(messages);
        }

        private async Task<dynamic> GetMessagesAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            var request = this.Bind<GetMessagesRequest>();

            if (await CheckTokenAsync(request.Token))
            {
                return Response.AsJson(new Error("Log in please"));
            }

            var messages = context.Messages.Where(m => m.SenderId == request.SenderId && m.TargetId == request.TargetId && m.ChannelId == request.ChannelId).Skip((int)parameters.skip).Take((int)parameters.limit).ToListAsync(cancellationToken);

            
            return Response.AsJson(messages);

        }

        private async Task<dynamic> SendMessageAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            var request = this.Bind<SendMessageRequest>();

            if(await CheckTokenAsync(request.Token))
            {
                return Response.AsJson(new Error("Log in please"));
            }

            var message = new Message
            {
                MessageText = request.MessageText,
                SenderId = request.SenderId,
                TargetId = request.TargetId,
                ChannelId = request.ChannelId
            };

            context.Add(message);
            await context.SaveChangesAsync(cancellationToken);
            return Response.AsJson(message);
        }
    }
}
