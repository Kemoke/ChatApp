using System;
using System.Collections.Generic;
using System.Text;
using ChatServer;
using ChatServer.Model;
using ChatServer.Request;
using ChatServer.Response;
using LightBDD.XUnit2;
using Nancy;
using Nancy.Responses.Negotiation;
using Nancy.Testing;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;


namespace ChatServerTests.Features
{
    public partial class Register_Feature : FeatureFixture
    {
        private readonly User user;
        protected readonly Browser Browser;
        protected readonly GlobalConfig Config;
        protected readonly ChatContext Context;
        private BrowserResponse result;

        #region Setup/Teardown
        public Register_Feature(ITestOutputHelper output) : base(output)
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

            user = DataGenerator.GenerateSingleUser(Context);

        }
        #endregion

        private void Given_the_user_enters_registration_information()
        {
            result = Browser.Post("/auth/register", with =>
            {
                with.Body(JsonConvert.SerializeObject(new RegisterRequest { User = user }), "application/json");
                with.Accept(new MediaRange("application/json"));
            }).Result;
        }

        private void Registration_is_successful()
        {
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        private void Existing_user_in_database()
        {
            result = Browser.Post("/auth/register", with =>
            {
                with.Body(JsonConvert.SerializeObject(new RegisterRequest { User = user }), "application/json");
                with.Accept(new MediaRange("application/json"));
            }).Result;
        }

        private void Registration_is_unsusccessful_because_of_existing_email()
        {
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            var response = JsonConvert.DeserializeObject<Error>(result.Body.AsString());
            Assert.Equal("Email already exists", response.Message);
        }
    }
}
