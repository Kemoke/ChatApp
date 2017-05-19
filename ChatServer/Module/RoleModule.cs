﻿using ChatServer.Model;
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
            Get("/", _ => "This is role module!!!!");
            Post("/assign_role", AssignRoleAsync);
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
    }
}
