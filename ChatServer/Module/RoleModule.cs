using ChatServer.Model;
using ChatServer.Request;
using ChatServer.Response;
using Nancy;
using Nancy.ModelBinding;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ChatServer.Module
{
    public class RoleModule : SecureModule
    {
        private readonly ChatContext context;

        public RoleModule(ChatContext context, GlobalConfig config) : base("/role", config)
        {
            this.context = context;
            Get("/", ListRoleAsync);
            Get("/{id}", GetRoleAsync);
            Post("/", CreateRoleAsync);
            Put("/{id}", EditRoleAsync);
            Delete("/{id}", DeleteRoleAsync);
            Post("/assign", AssignRoleAsync);
            Delete("/unsign", UnsignRoleAsync);
        }

        private async Task<dynamic> UnsignRoleAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            var request = this.Bind<UnsignRoleRequest>();
            var userteam = await context.UserTeams.FirstAsync(
                ut => ut.TeamId == request.TeamId 
                && ut.UserId == request.UserId, cancellationToken)
                .ConfigureAwait(false);
            context.UserTeams.Remove(userteam);

            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return Response.AsJson(new Msg("Role Unassigned"));
        }

        private async Task<dynamic> DeleteRoleAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            int id = parameters.id;
            context.Roles.Remove(new Role { Id = id });
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return Response.AsJson(new Msg("Role Deleted"));
        }

        private async Task<dynamic> EditRoleAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            var request = this.Bind<EditRoleRequest>();
            
            var role = await context.Roles.FindAsync((int) parameters.id).ConfigureAwait(false);

            role.Name = request.RoleName;

            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return Response.AsJson(role);
        }

        private async Task<dynamic> GetRoleAsync(dynamic parameters, CancellationToken cancellationToken)
        {
                int id = parameters.id;
                var channel = await context.Roles.AsNoTracking()
                    .FirstAsync(r => r.Id == id, cancellationToken)
                    .ConfigureAwait(false);
                return Response.AsJson(channel);
        }

        private async Task<dynamic> ListRoleAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            var roleList = await context.Roles.AsNoTracking()
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            var response = JsonConvert.SerializeObject(roleList);

            return Response.AsText(response, "application/json");
        }

        private async Task<dynamic> AssignRoleAsync(dynamic parameters, CancellationToken cancellationToken)
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

        private async Task<dynamic> CreateRoleAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            var request = this.Bind<CreateRoleRequest>();

            var role = new Role
            {
                Name = request.Name
            };

            context.Roles.Add(role);

            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return Response.AsJson(role);

        }
    }
}
