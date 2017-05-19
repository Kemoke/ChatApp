using ChatServer.Request;
using ChatServer.Response;
using Nancy;
using Nancy.ModelBinding;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChatServer.Module
{
    public class SettingsModule : SecureModule
    {
        private readonly ChatContext context;

        public SettingsModule(ChatContext context, GlobalConfig config) : base("/settings", config)
        {
            this.context = context;
            Get("/", _ => "This is settings Module!!!!");
            Post("/edit_info", EditInfoAsync);
            Post("/change_password", ChangePasswordAsync);
        }

        private async Task<dynamic> ChangePasswordAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            var request = this.Bind<ChangePasswordRequest>();

            var user = context.Users.Find(request.UserId);
            //ovo treba sve fino hashovati :D 
            if(BCrypt.Net.BCrypt.EnhancedVerify(request.OldPassword, user.Password))
            {
                user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(request.NewPassword);
                await context.SaveChangesAsync(cancellationToken);

                return Response.AsJson(new Error("Password changed successfully"));
            }
            return Response.AsJson(new Error("Wrong input for Old password"));
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

            return Response.AsJson(new Error("Data changed successfully"));
        }
    }
}
