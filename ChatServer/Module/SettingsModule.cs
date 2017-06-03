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
            Get("/", GetInfoAsync);

        }

        private Task<object> GetInfoAsync(object arg1, CancellationToken arg2)
        {
            throw new NotImplementedException();
        }

        
    }
}
