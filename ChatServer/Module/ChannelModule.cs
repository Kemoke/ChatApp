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
using Newtonsoft.Json;

namespace ChatServer.Module
{
    public class ChannelModule : SecureModule
    {
        private readonly ChatContext context;

        public ChannelModule(ChatContext context, GlobalConfig config) : base("/channel", config)
        {
            //ovako uzimaj config i context
            this.context = context;
            //list channels
            Get("/", ListChannelAsync);
            //get channel
            Get("/{id}", GetChannelAsync);
            //creates a new channel
            Post("/", CreateNewChannelAsync);
            //edit channel
            Put("/{id}", EditChannelAsync);
            //delete channel
            Delete("/{id}", DeleteChannelAsync);
            //saves message
            Post("/send", SendMessageAsync);
            //loads conversation
            Get("/messages/{skip}/{limit}", GetMessagesAsync);
            //checks if there are new messages
            Get("/messages/new", CheckNewMessagesAsync);
        }

        private async Task<dynamic> GetChannelAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            int id = parameters.id;
            var channel = await context.Channels.AsNoTracking().FirstAsync(c => c.Id == id, cancellationToken);

            return Response.AsJson(channel);
        }

        private async Task<dynamic> ListChannelAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            var request = this.Bind<ListChannelRequest>();

            var channelList = await context.Channels.AsNoTracking().Where(c => c.TeamId == request.TeamId).ToListAsync(cancellationToken);

            return Response.AsJson(channelList);
        }

        private async Task<dynamic> DeleteChannelAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            int id = parameters.id;

            context.Channels.Remove(new Channel {Id = id});

            await context.SaveChangesAsync(cancellationToken);

            return Response.AsJson(new Msg("Channel Deleted"));
        }

        private async Task<dynamic> EditChannelAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            
            int id = parameters.id;

            var request = this.Bind<Channel>();
            var channel = await context.Channels.FirstAsync(c => c.Id == id, cancellationToken);

            channel.ChannelName = request.ChannelName;
            await context.SaveChangesAsync(cancellationToken);

            return Response.AsJson(channel);
            
        }

        private async Task<dynamic> CreateNewChannelAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            var request = this.Bind<CreateChannelRequest>();

            if (!await IsUserAdminAsync(request.TeamId, request.UserId, cancellationToken))
            {
                return Response.AsJson(new Msg("You are not admin!")).WithStatusCode(HttpStatusCode.BadRequest);
            }

            if (await ChannelExistsAsync(request.ChannelName, request.TeamId))
            {
                return Response.AsJson(new Msg("Channel with that name already exists")).WithStatusCode(HttpStatusCode.BadRequest);
            }

            var channel = new Channel
            {
                TeamId = request.TeamId,
                ChannelName = request.ChannelName
            };

            context.Channels.Add(channel);
            await context.SaveChangesAsync(cancellationToken);
            return Response.AsJson(channel);
        }

        private async Task<bool> IsUserAdminAsync(int teamId, int userId, CancellationToken cancellationToken)
        {
            var roleId =  (await context.UserTeams.Where(ut => ut.UserId == userId && ut.TeamId == teamId).FirstAsync()).RoleId;

            return await context.Roles.Where(r => r.Id == roleId && r.Name == "Admin").AnyAsync(cancellationToken);
        }

        private async Task<bool> ChannelExistsAsync(string channelName, int teamId)
        {
            return await context.Channels.AnyAsync(c => c.ChannelName == channelName && c.TeamId == teamId);
        }

        private async Task<dynamic> CheckNewMessagesAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            //posaljemo mu ID zadnje poruke, provjerimo da li je to zadnja koja je spasena u bazi, ako nije onda vratimo sve ispred nje.

            var request = this.Bind<CheckNewMessagesRequest>();

            var messages = new List<Message>();
            
            var last = await context.Messages.AsNoTracking().Where(c => c.ChannelId == request.ChannelId).LastOrDefaultAsync(cancellationToken);
            var lastId = last is null ? 0 : last.Id;
            if (lastId != request.MessageId)
            {
                messages = await context.Messages.AsNoTracking().Include(m => m.Sender)
                    .Where(m => m.Id > request.MessageId && m.ChannelId == request.ChannelId)
                    .OrderByDescending(m => m.Id)
                    .ToListAsync(cancellationToken);
            }

            return Response.AsJson(messages);
        }

        private async Task<dynamic> GetMessagesAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            var request = this.Bind<GetMessagesRequest>();

            var messages = await context.Messages.AsNoTracking().Include(m => m.Sender)
                .Where(m => m.TargetId == request.TargetId || m.ChannelId == request.ChannelId)
                .Skip((int)parameters.skip)
                .Take((int)parameters.limit)
                .OrderByDescending(m => m.Id)
                .ToListAsync(cancellationToken);
            return Response.AsJson(messages);
        }

        private async Task<dynamic> SendMessageAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            var request = this.Bind<SendMessageRequest>();
            
            var message = new Message
            {
                MessageText = request.MessageText,
                SenderId = request.SenderId,
                TargetId = request.TargetId,
                ChannelId = request.ChannelId,
                TimeSent = DateTime.Now
            };

            context.Add(message);
            await context.SaveChangesAsync(cancellationToken);
            message.Sender = User;
            return Response.AsJson(message); 
        }
    }
}

