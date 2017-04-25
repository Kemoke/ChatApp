using Microsoft.AspNetCore.Builder;
using Nancy.Owin;

namespace ChatServer
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseOwin(x => x.UseNancy(new NancyOptions
            {
                Bootstrapper = new Bootstrapper(GlobalConfig.LoadConfig())
            }));
        }
    }
}