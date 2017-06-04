using ChatServer.Request;
using ChatServer.Response;
using Nancy;
using Nancy.ModelBinding;
using System;
using System.Threading;
using System.Threading.Tasks;
using ChatServer.Model;

namespace ChatServer.Module
{
    public class SettingsModule : SecureModule
    {
        private readonly ChatContext context;

        public SettingsModule(ChatContext context, GlobalConfig config) : base("/settings", config)
        {
            this.context = context;
            Get("/", GetInfoAsync);

        }

        private async Task<dynamic> GetInfoAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            var user = await context.Users.FindAsync(parameters.id);
            var userInfo = new UserInfo
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Company = user.Company,
                Country = user.Country,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender,
                PictureUrl = user.PictureUrl
            };

            return Response.AsJson(userInfo);
        }

        
    }
}
