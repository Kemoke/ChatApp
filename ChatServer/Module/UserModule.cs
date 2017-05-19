using ChatServer.Model;
using ChatServer.Request;
using ChatServer.Response;
using Microsoft.EntityFrameworkCore;
using Nancy;
using Nancy.ModelBinding;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChatServer.Module
{
    
    public class UserModule : SecureModule
    {
        private readonly ChatContext context;

        public UserModule(ChatContext context, GlobalConfig config) : base("/user", config)
        {
            this.context = context;
            Get("/",  _ => "This is user module!!!!");
            Get("/user_info", GetUserInfoAsync);
        }

        private async Task<dynamic> GetUserInfoAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            var request = this.Bind<GetUserInfoRequest>();

            //ovdje cu u model dodati samo jos jednu klasu(UserInfo) koja ce drzati informacije o useru

            var user = await context.Users.FindAsync(request.UserId);

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
