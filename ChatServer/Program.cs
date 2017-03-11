using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace ChatServer
{
    public class Program
    {
        public static void Main()
        {
            var host = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseKestrel()
                .UseStartup<Startup>()
                .Build();
            host.Start();
        }
    }
}