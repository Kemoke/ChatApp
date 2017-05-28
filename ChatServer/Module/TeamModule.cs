using Nancy;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ChatServer.Model;
using ChatServer.Request;
using ChatServer.Response;
using Microsoft.EntityFrameworkCore;
using Nancy.ModelBinding;

namespace ChatServer.Module
{
    public class TeamModule : SecureModule
    {
        private readonly ChatContext context;

        public TeamModule(GlobalConfig config, ChatContext context) : base("/team", config)
        {
            this.context = context;
            Get("/", _ => "This is team module!!!!");
            Post("/create_team", CreateTeamAsync);
        }

        private async Task<dynamic> CreateTeamAsync(dynamic arg, CancellationToken cancellationToken)
        {
            var request = this.Bind<CreateTeamRequest>();
            //neak bude i team name unique
            if (await TeamExistsAsync(request.Name))
            {
                return Response.AsJson(new Error("Channel with that name already exists"));
            }


            var team = new Team
            {
                Name = request.Name,
                UserId = request.UserId
            };

            context.Teams.Add(team);
            await context.SaveChangesAsync(cancellationToken);
            //uvjek vracaj objekt koji napravis preko apija nazad
            return Response.AsJson(team);
        }

        private async Task<bool> TeamExistsAsync(string teamName)
        {
            return await context.Teams.AnyAsync(t => t.Name == teamName);
        }
    }
}
