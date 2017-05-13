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
    
    public class UserModule : NancyModule
    {
        private readonly GlobalConfig config;
        private readonly ChatContext context;

        public UserModule() : base("/user")
        {
            Get("/",  _ => "This is user module!!!!");
            Post("/authenticate", AuthenticateAsync);
            Post("/register", RegisterAsync);
            Get("/log_out", LogOutAsync);
            Get("/user_info", GetUserInfoAsync);
        }

        private async Task<dynamic> GetUserInfoAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            var request = this.Bind<GetUserInfoRequest>();

            //ovdje cu u model dodati samo jos jednu klasu(UserInfo) koja ce drzati informacije o useru

            if (await CheckTokenAsync(request.Token))
            {
                return Response.AsJson(new Error("Log in please"));
            }

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

        private async Task<dynamic> LogOutAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            var request = this.Bind<BaseRequest>();
            if (await CheckTokenAsync(request.Token))
            {
                return Response.AsJson(new Error("Log in please"));
            }

            //deleteToken
            return Response.AsJson("Logged out");
        }

        private Task<bool> CheckTokenAsync(object token)
        {
            throw new NotImplementedException();
        }

        private async Task<dynamic> RegisterAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            var request = this.Bind<RegisterRequest>();

            if (!await EmailExistsAsync(request.Email))
            {
                var user = new User
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    //ovo treba hashovati
                    Password = request.Password,
                    Username = request.Username,
                    Company = request.Company,
                    Country = request.Country,
                    DateOfBirth = request.DateOfBirth,
                    PictureUrl = request.PictureUrl
                };

                context.Users.Add(user);

                await context.SaveChangesAsync(cancellationToken);

                return Response.AsJson(new Error("Registration successful"));
            }
            else
            {
                return Response.AsJson(new Error("Already existing Email"));
            }
            
        }

        private async Task<bool> EmailExistsAsync(string email)
        {
            return await context.Users.AnyAsync(u => u.Email == email);
        }

        //ovo ne znam za sta nam sluzi, nmg se sjetiti xd
        public async Task<dynamic> AuthenticateAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            return null;
        }
    }
}
