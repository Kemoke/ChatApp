using System;
using System.Collections.Generic;
using ChatServer;
using ChatServer.Model;
using Nancy.Testing;
using Xunit;

namespace ChatServerTests
{
    public class UserModuleTest
    {
        private readonly Browser browser;

        public UserModuleTest()
        {
            var config = new GlobalConfig
            {
                AppKey = "TestSecretKey",
                DbType = "inmemory",
                DbName = "ChatApp"
            };
            var bootstrapper = new Bootstrapper(config);
            browser = new Browser(bootstrapper);
            MockDataFactory.DbConfig = config;
            MockDataFactory.PopulateDatabase();
        }

        [Fact]
        public void GetSelfTest()
        {
            var response = browser.Get("/user/self");
            var contentType = response.Result.ContentType;
            var body = response.Result.Body.DeserializeJson<User>();
            Assert.Equal(contentType, "application/json");
        }
    }
}
