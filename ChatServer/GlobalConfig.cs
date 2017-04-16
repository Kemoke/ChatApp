using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace ChatServer
{
    public class GlobalConfig
    {
        public string DbName { get; set; }
        public string DbType { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string HostName { get; set; }
        public string AppKey { get; set; }

        public static GlobalConfig LoadConfig()
        {
            var configJson = File.ReadAllText(@"./config.json");
            return JsonConvert.DeserializeObject<GlobalConfig>(configJson);
        }
    }
}
