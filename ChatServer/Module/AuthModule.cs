using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ChatServer.Model;
using ChatServer.Request;
using ChatServer.Response;
using JWT;
using Microsoft.EntityFrameworkCore;
using Nancy;
using Nancy.ModelBinding;

namespace ChatServer.Module
{
    public class AuthModule : NancyModule
    {
        private readonly ChatContext context;
        private readonly GlobalConfig config;

        public AuthModule(ChatContext context, GlobalConfig config) : base("/auth")
        {
            this.context = context;
            this.config = config;
            Post("/login", LoginAsync);
            Post("/register", RegisterAsync);
        }

        private async Task<dynamic> LoginAsync(dynamic props, CancellationToken token)
        {
            try
            {
                var request = this.Bind<LoginRequest>();
                var user = await context.Users.AsNoTracking().FirstAsync(u => u.Username == request.Username, token);
                if (!BCrypt.Net.BCrypt.EnhancedVerify(request.Password, user.Password))
                {
                    return Response.AsJson(new Msg("Invalid credentials"))
                        .WithStatusCode(HttpStatusCode.Unauthorized);
                }
                user.Password = "";
                var time = DateTime.Now.AddDays(30).Ticks;
                var opts = new Dictionary<string, object>
                {
                    {"exp", time}
                };
                var jwt = JsonWebToken.Encode(opts, user, config.AppKey, JwtHashAlgorithm.HS512);
                return Response.AsJson(new LoginResponse
                {
                    Token = jwt,
                    User = user
                });
            }
            catch (Exception)
            {
                return Response.AsJson(new Msg("Invalid credentials")).WithStatusCode(HttpStatusCode.Unauthorized);
            }
        }

        private async Task<dynamic> RegisterAsync(dynamic props, CancellationToken token)
        {
            var request = this.Bind<RegisterRequest>();
            if (await EmailExistsAsync(request.User.Email))
            {
                return Response.AsJson(new Msg("Email already exists")).WithStatusCode(HttpStatusCode.BadRequest);
            }
            request.User.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(request.User.Password);
            context.Users.Add(request.User);
            await context.SaveChangesAsync(token);
            return Response.AsJson(new UserInfo
            {
                Username = request.User.Username,
                LastName = request.User.LastName,
                DateOfBirth = request.User.DateOfBirth,
                FirstName = request.User.FirstName,
                Company = request.User.Company,
                Gender = request.User.Gender,
                Country = request.User.Country,
                PictureUrl = request.User.PictureUrl
            });
        }

        private async Task<bool> EmailExistsAsync(string email)
        {
            return await context.Users.AnyAsync(u => u.Email == email);
        }
    }
}