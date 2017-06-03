using System;
using System.Collections.Generic;
using System.Text;
using ChatServer;
using ChatServer.Model;
using ChatServer.Request;
using ChatServer.Response;
using LightBDD.XUnit2;
using Microsoft.AspNetCore.Hosting.Internal;
using Nancy;
using Nancy.Responses.Negotiation;
using Nancy.Testing;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

[assembly: LightBddScope]

namespace ChatServerTests.Features
{
    public partial class Login_Feature : FeatureFixture
    {
       
        private readonly User user;
        protected readonly FeaturesConfig config;
        private BrowserResponse result;

        #region Setup/Teardown
        public Login_Feature(ITestOutputHelper output) : base(output)
        {
            
            config = new FeaturesConfig();

            user = DataGenerator.GenerateSingleUser(config.Context);

        }
        #endregion

        private void Given_the_user_is_already_registered()
        {
            result = config.Browser.Post("/auth/register", with =>
            {
                with.Body(JsonConvert.SerializeObject(new RegisterRequest { User = user }), "application/json");
                with.Accept(new MediaRange("application/json"));
            }).Result;
        }


        private void When_the_user_sends_login_request_with_correct_credentials()
        {
            result = config.Browser.Post("/auth/login", with =>
            {
                with.Body(JsonConvert.SerializeObject(new LoginRequest
                {
                    Username = user.Username,
                    Password = user.Password
                }), "application/json");
                with.Accept(new MediaRange("application/json"));
            }).Result;
        }

        private void Then_the_login_operation_should_be_successful()
        {
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            var body = result.Body.DeserializeJson<LoginResponse>();
            Assert.Equal(body.User.Username, user.Username);
            Assert.NotNull(body.Token);
            Assert.NotEmpty(body.Token);
        }

        private void When_the_user_sends_login_request_with_incorrect_username()
        {
            result = config.Browser.Post("/auth/login", with =>
            {
                with.Body(JsonConvert.SerializeObject(new LoginRequest
                {
                    Username = user.Username,
                    Password = user.Password
                }), "application/json");
                with.Accept(new MediaRange("application/json"));
            }).Result;
        }

        private void Then_the_login_operation_should_be_unsuccessful()
        {
            Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
            var response = JsonConvert.DeserializeObject<Error>(result.Body.AsString());
            Assert.Equal("Invalid credentials", response.Message);
        }

        private void When_the_user_sends_login_request_with_incorrect_password()
        {
            result = config.Browser.Post("/auth/login", with =>
            {
                with.Body(JsonConvert.SerializeObject(new LoginRequest
                {
                    Username = user.Username,
                    Password = "sdadada",
                }), "application/json");
                with.Accept(new MediaRange("application/json"));
            }).Result;
        }
    }
}
