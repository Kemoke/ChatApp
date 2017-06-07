using System.Linq;
using ChatServer.Model;
using ChatServer.Response;
using JWT;
using Nancy;

namespace ChatServer.Module
{
    public abstract class SecureModule : NancyModule
    {
        protected User User;

        protected SecureModule(string path, GlobalConfig config) : base(path)
        {
            Before += ctx =>
            {
                try
                {
                    var jwt = ctx.Request.Headers["Authorization"].First();
                    User = JsonWebToken.DecodeToObject<User>(jwt, config.AppKey);
                    return null;
                }
                catch (SignatureVerificationException e)
                {
                    return Response.AsJson(new Msg("Not authorized")).WithStatusCode(HttpStatusCode.Unauthorized);
                }
            };

        }
    }
}