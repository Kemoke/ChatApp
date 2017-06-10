using System;
using ChatServer.WebSockets;
using JWT;
using Microsoft.Extensions.DependencyInjection;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Configuration;
using Nancy.TinyIoc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ChatServer
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        private readonly GlobalConfig config;
        private readonly IServiceProvider provider;

        public Bootstrapper(GlobalConfig config, IServiceProvider provider)
        {
            this.config = config;
            this.provider = provider;
        }

        public Bootstrapper(GlobalConfig config)
        {
            this.config = config;
            this.provider = null;
        }

        public override void Configure(INancyEnvironment environment)
        {
            base.Configure(environment);
            environment.Tracing(true, true);
        }

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);
            var serializer = new JsonSerializer
            {
                Formatting = Formatting.None,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.None
            };
            container.Register(serializer);
            if (provider != null)
            {
                container.Register((c, p) => provider.GetService<NotificationsMessageHandler>());
            }
            JsonWebToken.JsonSerializer = new JwtSerializer();
            container.Register(config);
            using (var context = new ChatContext(config))
            {
                context.Database.EnsureCreated();
            }
            pipelines.AfterRequest += ctx =>
            {
                ctx.Response
                    .WithHeader("Access-Control-Allow-Origin", "*")
                    .WithHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS")
                    .WithHeader("Access-Control-Allow-Headers", "Accept, Origin, Content-type, Authorization");
            };
        }

        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);
            container.Register<ChatContext>();
        }
    }
}