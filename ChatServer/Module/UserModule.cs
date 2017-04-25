using Nancy;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChatServer.Module
{
    
    public class UserModule : NancyModule
    {
        public UserModule() : base("/user")
        {
            Get("/",  _ => "This si user module!!!!");
            Post("/authenticate", Authenticate);
            Post("/register", Register);
            Post("/log_out", LogOut);
            Post("/user_info", GetUserInfo);
        }

        private async Task<dynamic> GetUserInfo(dynamic parameters, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private async Task<dynamic> LogOut(dynamic parameters, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private async Task<dynamic> Register(dynamic parameters, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<dynamic> Authenticate(dynamic parameters, CancellationToken cancellationToken)
        {
            return null;
        }
    }
}
