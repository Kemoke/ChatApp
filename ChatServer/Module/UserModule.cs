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

        private Task<object> GetSelfAsync(object arg1, CancellationToken arg2)
        {
            dynamic parameters = new ExpandoObject();
            parameters.id = User.Id;
            return GetUserInfoAsync(parameters, arg2);
        }

        private Task<object> ListUsersAsync(object arg1, CancellationToken arg2)
        {
            throw new NotImplementedException();
        }

        private async Task<dynamic> GetUserInfoAsync(dynamic parameters, CancellationToken cancellationToken)
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

        private async Task<dynamic> ChangePasswordAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            var request = this.Bind<ChangePasswordRequest>();

            var user = context.Users.Find(request.UserId);
            if (!BCrypt.Net.BCrypt.EnhancedVerify(request.OldPassword, user.Password))
            {
                return Response.AsJson(new Error("Wrong input for Old password"));
            }
            user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(request.NewPassword);
            await context.SaveChangesAsync(cancellationToken);

            return Response.AsText("Password changed successfully");
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

            return Response.AsText("Data changed successfully");
        }
    }
}
