using ChatServer.Model;
using ChatServer.Request;
using ChatServer.Response;
using Nancy;
using Nancy.ModelBinding;
using System;
using System.Threading;
using System.Threading.Tasks;

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
            Post("/assign", AssignRoleAsync);
            Post("/", CreateRoleAsync);
        }

        private Task<object> GetRoleAsync(object arg1, CancellationToken arg2)
        {
            throw new NotImplementedException();
        }

        private Task<object> ListRoleAsync(object arg1, CancellationToken arg2)
        {
            throw new NotImplementedException();
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
