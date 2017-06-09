using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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
    public partial class LoginFeature : FeatureFixture
    {
       
        private readonly User user;
        protected readonly FeaturesConfig config;
        private BrowserResponse result;
        private readonly FeatureHelper helper;

        #region Setup/Teardown
        public LoginFeature(ITestOutputHelper output) : base(output)
        {
            
            config = new FeaturesConfig();

            user = DataGenerator.GenerateSingleUser(config.Context);

            helper = new FeatureHelper(config);

        }
        #endregion

        private async Task Given_the_user_is_already_registered()
        {
            result = await helper.RegisterResponse(user);
        }


        private async Task When_the_user_sends_login_request_with_correct_credentials()
        {
            result = await helper.LoginResponse(user);
        }

        private Task Then_the_login_operation_should_be_successful()
        {
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            var body = result.Body.DeserializeJson<LoginResponse>();
            Assert.Equal(body.User.Username, user.Username);
            Assert.NotNull(body.Token);
            Assert.NotEmpty(body.Token);
            return Task.CompletedTask;
        }

        private async Task When_the_user_sends_login_request_with_incorrect_username()
        {
            var user2 = DataGenerator.GenerateSingleUser(config.Context);
            result = await helper.LoginResponse(user2);
        
        }

        private Task Then_the_login_operation_should_be_unsuccessful()
        {
            Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
            var response = result.BodyJson<Msg>();
            Assert.Equal("Invalid credentials", response.Message);
            return Task.CompletedTask;
        }

        private async Task When_the_user_sends_login_request_with_incorrect_password()
        {
            result = await config.Browser.Post("/auth/login", with =>
            {
                with.BodyJson(new LoginRequest
                {
                    Username = user.Username,
                    Password = "invalid password"
                });
            });
        }
    }
}
