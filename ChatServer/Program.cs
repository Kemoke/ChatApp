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
            Console.WriteLine("Loading config");
            var conf = GlobalConfig.LoadConfig();
            Console.WriteLine($"Starting server at {conf.HostName}");
            var host = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseKestrel()
                .UseUrls(conf.HostName)
                .UseStartup<Startup>()
                .Build();
            host.Start();
            Console.WriteLine($"Server started at {conf.HostName}");
            Thread.Sleep(Timeout.Infinite);
        }
    }
}