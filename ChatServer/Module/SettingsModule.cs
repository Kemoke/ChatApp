using Nancy;
using System;

namespace ChatServer.Module
{
    public class SettingsModule : NancyModule
    {
        public SettingsModule() : base("/settings")
        {
            Get("/", _ => "This is settings Module!!!!");
            Post("/edit_info", parameters => EditInfo(parameters));
            Post("/change_password", parameters => ChangePassword(parameters));
            Post("/change_picture", parameters => ChangePicture(parameters));
        }

        private dynamic ChangePicture(dynamic parameters)
        {
            throw new NotImplementedException();
        }

        private dynamic ChangePassword(dynamic parameters)
        {
            throw new NotImplementedException();
        }

        private dynamic EditInfo(dynamic parameters)
        {
            throw new NotImplementedException();
        }
    }
}
