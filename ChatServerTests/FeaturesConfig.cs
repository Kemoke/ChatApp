using System;
using System.Collections.Generic;
using System.Text;
using ChatServer;
using Nancy.Testing;

namespace ChatServerTests
{
    public class FeaturesConfig
    {
        //protected readonly Browser Browser;
        public Browser Browser { get; set; }
        public GlobalConfig Config { get; set; }
        //protected readonly GlobalConfig Config;
        public ChatContext Context { get; set; }
        //protected readonly ChatContext Context;

        public FeaturesConfig()
        {
            Config = new GlobalConfig
            {
                AppKey = "TestSecretKey",
                DbType = "inmemory",
                DbName = "ChatApp"
            };
            var bootstrapper = new Bootstrapper(Config);
            Context = new ChatContext(Config);
            Browser = new Browser(bootstrapper);
        }
    }
}
