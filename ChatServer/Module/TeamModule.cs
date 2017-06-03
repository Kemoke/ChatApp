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
            try
            {
                var request = this.Bind<CreateTeamRequest>();
                //neak bude i team name unique
                if (await TeamExistsAsync(request.Name))
                {
                    return Response.AsJson(new Error("Channel with that name already exists")).WithStatusCode(HttpStatusCode.BadRequest);
                }


                var team = new Team
                {
                    Name = request.Name
                };

                context.Teams.Add(team);

                await context.SaveChangesAsync(cancellationToken);

                return Response.AsJson(team);
            }
            catch (Exception e)
            {
                return Response.AsJson(new Error(e.Message)).WithStatusCode(HttpStatusCode.BadRequest);
            }
           
        }

        private async Task<bool> TeamExistsAsync(string teamName)
        {
            return await context.Teams.AnyAsync(t => t.Name == teamName);
        }
    }
}
