using System;
using System.Collections.Generic;
using System.Text;
using ChatServer;
using Nancy.Testing;
using Xunit;


namespace ChatServerTests
{
    public class FeaturesConfig
    {
        public Browser Browser { get; }
        public GlobalConfig Config { get; }
        public ChatContext Context { get; }

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
