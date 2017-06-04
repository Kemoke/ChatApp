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
            Delete("/unassign", UnassignRoleAsync);
        }

        private async Task<dynamic> UnassignRoleAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            var request = this.Bind<UnassignRoleRequest>();

            context.UserTeams.Remove(new UserTeam
            {
                TeamId = request.TeamId,
                UserId = request.UserId,
                RoleId = request.RoleId
            });

            await context.SaveChangesAsync(cancellationToken);
            return Response.AsText("Role Unassigned");
        }

        private async Task<dynamic> DeleteRoleAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            int id = parameters.id;
            context.Roles.Remove(new Role { Id = id });
            await context.SaveChangesAsync(cancellationToken);
            return Response.AsText("Role Deleted");
        }

        private async Task<dynamic> EditRoleAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            var request = this.Bind<EditRoleRequest>();
            
            var role = await context.Roles.FindAsync((int) parameters.id);

            role.Name = request.RoleName;

            await context.SaveChangesAsync(cancellationToken);

            return Response.AsJson(new Msg("Data changed successfully"));
        }

        private async Task<dynamic> GetRoleAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            try
            {
                int id = parameters.id;
                var channel = await context.Roles.AsNoTracking().FirstAsync(r => r.Id == id, cancellationToken);

                return Response.AsJson(channel);
            }
            catch (Exception e)
            {
                return Response.AsJson(new Msg("Something went wrong")).WithStatusCode(HttpStatusCode.BadRequest);
            }
        }

        private async Task<object> ListRoleAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            var roleList = await context.Channels.AsNoTracking().ToListAsync(cancellationToken);

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

            await context.SaveChangesAsync(cancellationToken);

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

            await context.SaveChangesAsync(cancellationToken);

            return Response.AsJson(role);

        }
    }
}
