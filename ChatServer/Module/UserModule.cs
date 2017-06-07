using ChatServer.Model;
using ChatServer.Request;
using ChatServer.Response;
using Microsoft.EntityFrameworkCore;
using Nancy;
using Nancy.ModelBinding;
using System;
using System.Dynamic;
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
            Get("/", ListUsersAsync);
            Get("/{id}", GetUserInfoAsync);
            Get("/self", GetSelfAsync);
            Put("/", EditInfoAsync);
            Post("/change_password", ChangePasswordAsync);
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        private async Task<dynamic> GetSelfAsync(dynamic parameters, CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            return Response.AsJson(User);
        }

        private async Task<dynamic> ListUsersAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            var users = await context.Users.AsNoTracking().ToListAsync(cancellationToken);
            return Response.AsJson(users);
        }

        private async Task<dynamic> GetUserInfoAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            int id = parameters.id;

            var user = await context.Users.AsNoTracking().FirstAsync(u => u.Id == id, cancellationToken);

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

        private async Task<dynamic> ChangePasswordAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            var request = this.Bind<ChangePasswordRequest>();

            var user = context.Users.Find(request.UserId);
            if (!BCrypt.Net.BCrypt.EnhancedVerify(request.OldPassword, user.Password))
            {
                return Response.AsJson(new Msg("Wrong input for old password")).WithStatusCode(HttpStatusCode.BadRequest);
            }
            user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(request.NewPassword);
            await context.SaveChangesAsync(cancellationToken);

            return Response.AsJson(new Msg("Password changed successfully"));
        }

        private async Task<dynamic> EditInfoAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            var request = this.Bind<EditUserInfoRequest>();

            User = await context.Users.FindAsync(User.Id);

            User.Company = request.Company;
            User.Country = request.Country;
            User.DateOfBirth = request.DateOfBirth;
            User.FirstName = request.FirstName;
            User.LastName = request.LastName;
            User.Gender = request.Gender;
            User.PictureUrl = request.PictureUrl;
            User.Username = request.Username;

            await context.SaveChangesAsync(cancellationToken);

            return Response.AsJson(new Msg("Data changed successfully"));   
        }
    }
}
