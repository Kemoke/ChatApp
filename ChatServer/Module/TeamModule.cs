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
            Get("/", ListTeamAsync);
            Get("/{id}", GetTeamAsync);
            Post("/", CreateTeamAsync);
            Put("/{id}", EditTeamAsync);
            Delete("/{id}", DeleteTeamAsync);
            Post("/user/add", AddUserAsync);
            Delete("/user/remove", RemoveUserAsync);
        }

        private async Task<dynamic> RemoveUserAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            var request = this.Bind<UnsignRoleRequest>();
            var userteam = await context.UserTeams.FirstAsync(
                ut => ut.TeamId == request.TeamId && ut.UserId == request.UserId, cancellationToken).ConfigureAwait(false);
            context.UserTeams.Remove(userteam);

            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return Response.AsJson(new Msg("User Removed"));
        }

        private async Task<dynamic> AddUserAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            var request = this.Bind<AssignRoleRequest>();

            var userRole = new UserTeam
            {
                RoleId = request.RoleId,
                TeamId = request.TeamId,
                UserId = request.UserId
            };

            context.UserTeams.Add(userRole);

            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            
            return Response.AsJson(userRole);
        }

        private async Task<dynamic> DeleteTeamAsync(dynamic props, CancellationToken token)
        {
            int id = props.id;
            context.Teams.Remove(new Team {Id = id});
            await context.SaveChangesAsync(token).ConfigureAwait(false);
            return Response.AsJson(new Msg("Deleted"));
        }

        private async Task<dynamic> EditTeamAsync(dynamic props, CancellationToken token)
        {
            int id = props.id;
            var request = this.Bind<Team>();
            var team = await context.Teams.FirstAsync(t => t.Id == id, token).ConfigureAwait(false);
            team.Name = request.Name;
            await context.SaveChangesAsync(token).ConfigureAwait(false);
            return Response.AsJson(team);
        }

        private async Task<object> GetTeamAsync(dynamic props, CancellationToken token)
        {
            int id = props.id;
            var team = await context.Teams.AsNoTracking().Include(t => t.UserTeams)
                .ThenInclude(t => t.User)
                .FirstAsync(u => u.Id == id, token).ConfigureAwait(false);
            return Response.AsJson(team);
        }

        private async Task<dynamic> ListTeamAsync(dynamic props, CancellationToken token)
        {
            var teams = await context.Teams.AsNoTracking().Include(t => t.UserTeams)
                .ThenInclude(t => t.User)
                .ToListAsync(token).ConfigureAwait(false);
            return Response.AsJson(teams);
        }

        private async Task<dynamic> CreateTeamAsync(dynamic arg, CancellationToken cancellationToken)
        {
                var request = this.Bind<CreateTeamRequest>();
                if (await TeamExistsAsync(request.Name).ConfigureAwait(false))
                {
                    return Response.AsJson(new Msg("Channel with that name already exists")).WithStatusCode(HttpStatusCode.BadRequest);
                }


                var team = new Team
                {
                    Name = request.Name
                };

                context.Teams.Add(team);

                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                return Response.AsJson(team);
        }

        private async Task<bool> TeamExistsAsync(string teamName)
        {
            return await context.Teams.AnyAsync(t => t.Name == teamName).ConfigureAwait(false);
        }
    }
}
