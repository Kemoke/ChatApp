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
           try
           {
               var channel = await context.Channels.FindAsync((int)parameters.id, cancellationToken);

               return Response.AsJson(channel);
           }
           catch(Exception e)
           {
               return Response.AsJson(new Error("Something went wrong")).WithStatusCode(HttpStatusCode.BadRequest);
           }
            
        }

        private async Task<dynamic> ListChannelAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            var request = this.Bind<ListChannelRequest>();

            var channelList = await context.Channels.Where(c => c.TeamId == request.TeamId).ToListAsync(cancellationToken);

            var response = JsonConvert.SerializeObject(channelList);
            return Response.AsText(response, "application/json");
        }

        private async Task<dynamic> DeleteChannelAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            try
            {
                var channel = await context.Channels.FindAsync((int)parameters.id); 

                context.Channels.Remove(channel);

                await context.SaveChangesAsync(cancellationToken);

                return Response.AsJson(channel);
            }
            catch (Exception e)
            {
                return Response.AsJson(new Error("Something went wrong")).WithStatusCode(HttpStatusCode.BadRequest);
            }
        }

        private async Task<dynamic> EditChannelAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            try
            {
                var request = this.Bind<EditChannelInfoRequest>();

                var channel = await context.Channels.FindAsync(parameters.Id);

                channel.ChannelName = request.ChannelName;

                await context.SaveChangesAsync(cancellationToken);

                return Response.AsJson("Data changed successfully");
            }
            catch (Exception e)
            {
                return Response.AsJson(new Error("Something went wrong")).WithStatusCode(HttpStatusCode.BadRequest);
            }

        }

        private async Task<dynamic> CreateNewChannelAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            var request = this.Bind<CreateChannelRequest>();
            //sve sto radi sa bazom koristi async metode i await ispred

            if (await IsUserAdminAsync(request.TeamId, request.UserId))
            {
                return Response.AsJson(new Error("You are not admin!")).WithStatusCode(HttpStatusCode.BadRequest);
            }

            if (await ChannelExistsAsync(request.ChannelName, request.TeamId))
            {
                return Response.AsJson(new Error("Channel with that name already exists")).WithStatusCode(HttpStatusCode.BadRequest);
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

        private async Task<bool> IsUserAdminAsync(int teamId, int userId)
        {
            return await context.UserTeams.AnyAsync(ut => ut.UserId == userId && ut.TeamId == teamId && ut.RoleId == 1);
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
            
            if (await context.Messages.AnyAsync(cancellationToken))
            {
                var lastId = (await context.Messages.LastAsync(cancellationToken)).Id;
                if (lastId == request.MessageId)
                {
                    return Response.AsJson(messages);
                }
            }
            else
            {
                return Response.AsJson(new Error("There are no messages to be dipslayed"));
            }
            

            messages = await context.Messages.Where(m => (m.Id > request.MessageId && m.Id == request.ChannelId)).ToListAsync(cancellationToken);

            return Response.AsJson(messages);
        }

        private async Task<dynamic> GetMessagesAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            var request = this.Bind<GetMessagesRequest>();

            var messages = await context.Messages.Where(m => m.SenderId == request.SenderId && m.TargetId == request.TargetId && m.ChannelId == request.ChannelId).Skip((int)parameters.skip).Take((int)parameters.limit).ToListAsync(cancellationToken);
            var response = JsonConvert.SerializeObject(messages);
            return Response.AsText(response, "application/json");
        }

        private async Task<dynamic> SendMessageAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            var request = this.Bind<SendMessageRequest>();
            try
            {
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
            catch(Exception e)
            {
                return Response.AsJson(new Error(e.Message)).WithStatusCode(HttpStatusCode.BadRequest);
            }
            
        }
    }
}

