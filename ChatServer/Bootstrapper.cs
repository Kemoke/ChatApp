using System;
using JWT;
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

        public Bootstrapper(GlobalConfig config)
        {
            this.config = config;
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
            JsonWebToken.JsonSerializer = new JwtSerializer();
            container.Register(config);
            using (var context = new ChatContext(config))
            {
                context.Database.EnsureCreated();
            }
        }

        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);
            container.Register<ChatContext>().UsingConstructor(()=> new ChatContext(config)).AsMultiInstance();
        }
    }
}