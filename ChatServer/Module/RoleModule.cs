using Nancy;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChatServer.Module
{
    public class RoleModule : NancyModule
    {
        public RoleModule() : base("/role")
        {
            Get("/", _ => "This is role module!!!!");
            Post("/assign_role", AssignRoleAsync);
        }

        private async Task<dynamic> AssignRoleAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
