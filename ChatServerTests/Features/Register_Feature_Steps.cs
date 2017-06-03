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


namespace ChatServerTests.Features
{
    public partial class Register_Feature : FeatureFixture
    {
        private readonly User user;
        protected readonly FeaturesConfig config;
        private BrowserResponse result;

        #region Setup/Teardown
        public Register_Feature(ITestOutputHelper output) : base(output)
        {

            config = new FeaturesConfig();
            
            user = DataGenerator.GenerateSingleUser(config.Context);

        }
        #endregion

        private void Given_the_user_enters_registration_information()
        {
            result = config.Browser.Post("/auth/register", with =>
            {
                with.BodyJson(new RegisterRequest { User = user });
                with.Accept(new MediaRange("application/json"));
            }).Result;
        }

        private void Registration_is_successful()
        {
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        private void Existing_user_in_database()
        {
            result = config.Browser.Post("/auth/register", with =>
            {
                with.BodyJson(new RegisterRequest { User = user });
                with.Accept(new MediaRange("application/json"));
            }).Result;
        }

        private void Registration_is_unsusccessful_because_of_existing_email()
        {
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            var response = result.BodyJson<Error>();
            Assert.Equal("Email already exists", response.Message);
        }
    }
}
