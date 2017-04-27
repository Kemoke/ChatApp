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
            Post("/authenticate", AuthenticateAsync);
            Post("/register", RegisterAsync);
            Post("/log_out", LogOutAsync);
            Post("/user_info", GetUserInfoAsync);
        }

        private async Task<dynamic> GetUserInfoAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private async Task<dynamic> LogOutAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private async Task<dynamic> RegisterAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<dynamic> AuthenticateAsync(dynamic parameters, CancellationToken cancellationToken)
        {
            return null;
        }
    }
}
