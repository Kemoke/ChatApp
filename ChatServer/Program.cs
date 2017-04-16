using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel;

namespace ChatServer
{
    public class Program
    {
        public static void Main()
        {
            var conf = GlobalConfig.LoadConfig();
            var host = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseKestrel()
                .UseUrls(conf.HostName)
                .UseStartup<Startup>()
                .Build();
            host.Start();
        }
    }
}