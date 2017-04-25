using System;
using System.IO;
using System.Threading;
using Microsoft.AspNetCore.Hosting;

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
            Thread.Sleep(Timeout.Infinite);
        }
    }
}