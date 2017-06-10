using System;
using ChatServer.WebSockets;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Nancy.Owin;

namespace ChatServer
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app, IServiceProvider serviceProvider)
        {
            app.UseWebSockets();
            app.MapWebSocketManager("/notifications", serviceProvider.GetService<NotificationsMessageHandler>());
            app.UseOwin(x => x.UseNancy(new NancyOptions
            {
                Bootstrapper = new Bootstrapper(GlobalConfig.LoadConfig(), serviceProvider)
            }));
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddWebSocketManager();
        }
    }
}