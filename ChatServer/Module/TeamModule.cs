using Nancy;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatServer.Module
{
    public class TeamModule : NancyModule
    {
        private readonly GlobalConfig config;
        private readonly ChatContext context;


        public TeamModule() : base("/team")
        {
            Get("/", _ => "This is team module!!!!");
            Post("/create_team", CreateTeamAsync);
        }

        private async Task<dynamic> CreateTeamAsync(dynamic arg, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
