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
            Post("/edit_info", EditInfo);
            Post("/change_password", ChangePassword);
            Post("/change_picture", ChangePicture);
        }

        private async Task<dynamic> ChangePicture(dynamic parameters, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private async Task<dynamic> ChangePassword(dynamic parameters, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private async Task<dynamic> EditInfo(dynamic parameters, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
