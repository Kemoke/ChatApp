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
        protected readonly GlobalConfig config;
        protected readonly ChatContext context;

        protected HttpTest()
        {
            config = new GlobalConfig
            {
                AppKey = "TestSecretKey",
                DbType = "inmemory",
                DbName = "ChatApp"
            };
            var bootstrapper = new Bootstrapper(config);
            context = new ChatContext(config);
            Browser = new Browser(bootstrapper);
        }
    }
}
