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
    public class RoleModule : NancyModule
    {
        private readonly GlobalConfig config;
        private readonly ChatContext context;

        public RoleModule() : base("/role")
        {
            Get("/", _ => "This is role module!!!!");
            Post("/assign_role", AssignRoleAsync);
        }

        private async Task<dynamic> AssignRoleAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            var request = this.Bind<AssignRoleRequest>();

            if (!await CheckTokenAsync(request.Token))
            {
                return Response.AsJson(new Error("Log in please"));
            }


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


        private Task<bool> CheckTokenAsync(Token token)
        {
            throw new NotImplementedException();
        }
    }
}
