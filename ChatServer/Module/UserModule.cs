using Nancy;
using Nancy.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatServer.Module
{
    
    public class UserModule : NancyModule
    {
        public UserModule() : base("/user")
        {
            Get("/",  _ => "This si user module!!!!");
            Post("/authenticate", parameters => Authenticate(parameters));
            Post("/register", parameters => Register(parameters));
            Post("/log_out", parameters => LogOut(parameters));
            Post("/user_info", parameters => GetUserInfo(parameters));
        }

        private dynamic GetUserInfo(dynamic parameters)
        {
            throw new NotImplementedException();
        }

        private dynamic LogOut(dynamic parameters)
        {
            throw new NotImplementedException();
        }

        private dynamic Register(dynamic parameters)
        {
            throw new NotImplementedException();
        }

        public dynamic Authenticate(dynamic parameters)
        {
            return null;
        }
    }
}
