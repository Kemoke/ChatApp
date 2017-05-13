using Nancy;
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
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);
            container.Register(new JsonSerializer
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.None,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.None
            });
            container.Register(config);
            using (var context = new ChatContext(config))
            {
                context.Database.EnsureCreated();
            }
        }
    }
}