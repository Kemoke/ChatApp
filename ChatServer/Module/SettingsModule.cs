using ChatServer.Request;
using ChatServer.Response;
using Nancy;
using Nancy.ModelBinding;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChatServer.Module
{
    public class SettingsModule : NancyModule
    {
        private readonly GlobalConfig config;
        private readonly ChatContext context;

        public SettingsModule() : base("/settings")
        {
            Get("/", _ => "This is settings Module!!!!");
            Post("/edit_info", EditInfoAsync);
            Post("/change_password", ChangePasswordAsync);
            //Post("/change_picture", ChangePictureAsync);
        }
        //ovo je sada bespotrebno, jer sam stavio da se slika mijenja u EditInfoAsync
        /*private async Task<dynamic> ChangePictureAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }*/

        private async Task<dynamic> ChangePasswordAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            var request = this.Bind<ChangePasswordRequest>();

            if (!await CheckTokenAsync(request.Token))
            {
                return Response.AsJson(new Error("Log in please"));
            }

            var user = context.Users.Find(request.UserId);
            //ovo treba sve fino hashovati :D 
            if(user.Password == request.OldPassword)
            {
                user.Password = request.NewPassword;
                await context.SaveChangesAsync(cancellationToken);

                return Response.AsJson(new Error("Password changed successfully"));
            }
            else
            {
                return Response.AsJson(new Error("Wrong input for Old password"));
            }

        }

        private async Task<dynamic> EditInfoAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            var request = this.Bind<EditUserInfoRequest>();

            if (!await CheckTokenAsync(request.Token))
            {
                return Response.AsJson(new Error("Log in please"));
            }

            var user = context.Users.Find(request.UserId);

            user.Company = request.Company;
            user.Country = request.Country;
            user.DateOfBirth = request.DateOfBirth;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Gender = request.Gender;
            user.PictureUrl = request.PictureUrl;
            user.Username = request.Username;

            await context.SaveChangesAsync(cancellationToken);

            return Response.AsJson(new Error("Data changed successfully"));
        }

        private Task<bool> CheckTokenAsync(object token)
        {
            throw new NotImplementedException();
        }
    }
}
