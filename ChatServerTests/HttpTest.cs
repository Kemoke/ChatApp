using System;
using System.Collections.Generic;
using ChatServer;
using ChatServer.Model;
using Nancy.Testing;
using Xunit;

namespace ChatServerTests
{
    public abstract class HttpTest
    {
        protected readonly Browser Browser;
        protected readonly GlobalConfig Config;
        protected readonly ChatContext Context;

        protected HttpTest()
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
