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


namespace ChatServerTests.Features
{
    public partial class RegisterFeature : FeatureFixture
    {
        private readonly User user;
        protected readonly FeaturesConfig config;
        private BrowserResponse result;
        private readonly FeatureHelper helper;

        #region Setup/Teardown
        public RegisterFeature(ITestOutputHelper output) : base(output)
        {

            config = new FeaturesConfig();
            
            user = DataGenerator.GenerateSingleUser(config.Context);

            helper = new FeatureHelper(config);
        }
        #endregion

        private async Task Given_the_user_enters_registration_information()
        {
            result = await helper.RegisterResponse(user);
        }

        private async Task Registration_is_successful()
        {
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        private async Task Existing_user_in_database()
        {
            result = await helper.RegisterResponse(user);
        }

        private Task Registration_is_unsusccessful_because_of_existing_email()
        {
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            var response = result.BodyJson<Msg>();
            Assert.Equal("Email already exists", response.Message);
            return Task.CompletedTask;
        }
    }
}
