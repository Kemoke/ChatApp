using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace ChatServer.WebSockets
{
    public static class Extensions
    {
        public static IApplicationBuilder MapWebSocketManager(this IApplicationBuilder app,
            PathString path,
            WebSocketHandler handler)
        {
            return app.Map(path, (ap) => ap.UseMiddleware<WebSocketMiddleware>(handler));
        }

        public static IServiceCollection AddWebSocketManager(this IServiceCollection services)
        {
            services.AddTransient<WebSocketConnectionManager>();

            foreach (var type in Assembly.GetEntryAssembly().ExportedTypes)
            {
                if (type.GetTypeInfo().BaseType == typeof(WebSocketHandler))
                {
                    services.AddSingleton(type);
                }
            }

            return services;
        }
    }
}