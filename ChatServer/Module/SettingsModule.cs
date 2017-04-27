using Nancy;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChatServer.Module
{
    public class SettingsModule : NancyModule
    {
        public SettingsModule() : base("/settings")
        {
            Get("/", _ => "This is settings Module!!!!");
            Post("/edit_info", EditInfoAsync);
            Post("/change_password", ChangePasswordAsync);
            Post("/change_picture", ChangePictureAsync);
        }

        private async Task<dynamic> ChangePictureAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private async Task<dynamic> ChangePasswordAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private async Task<dynamic> EditInfoAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
