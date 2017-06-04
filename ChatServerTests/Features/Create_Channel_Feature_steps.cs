using System;
using System.Collections.Generic;
using System.Text;
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
    public partial class Create_Channel_Feature : FeatureFixture
    {
        private readonly User user;
        private Team team;
        private readonly FeaturesConfig config;
        private BrowserResponse createTeamResult;
        private BrowserResponse addRoleResult;
        private BrowserResponse createChannelResult;
        private BrowserResponse createNewChannelResult;
        private BrowserResponse loginResult;



        #region Setup/Teardown

        public Create_Channel_Feature(ITestOutputHelper output) : base(output)
        {

            config = new FeaturesConfig();

            user = DataGenerator.GenerateSingleUser(config.Context);

            
        }

        #endregion

        private void Given_the_user_is_logged_in()
        {
            loginResult = config.Browser.Post("/auth/register", with =>
            {
                with.BodyJson(new RegisterRequest { User = user });
                with.Accept(new MediaRange("application/json"));
            }).Result;

            loginResult = config.Browser.Post("/auth/login", with =>
            {
                with.BodyJson(new LoginRequest
                {
                    Username = user.Username,
                    Password = user.Password
                });
                with.Accept(new MediaRange("application/json"));
            }).Result;


            Assert.Equal(HttpStatusCode.OK, loginResult.StatusCode);
            var body = loginResult.BodyJson<LoginResponse>();
            Assert.Equal(body.User.Username, user.Username);
            Assert.NotNull(body.Token);
            Assert.NotEmpty(body.Token);

        }

        private void Given_the_user_creates_team_and_is_admin()
        {
            team = DataGenerator.GenerateSingleTeam(config.Context);
        }
        
        private void User_tries_to_create_new_channel_providing_channel_name()
        {
            createChannelResult = config.Browser.Post("/channel/", with =>
                {
                    with.BodyJson(new CreateChannelRequest
                    {
                        ChannelName = "Developer",
                        UserId = loginResult.Body.DeserializeJson<LoginResponse>().User.Id,
                        TeamId = team.Id
                    });
                    with.Accept(new MediaRange("application/json"));
                    with.Header("Authorization", loginResult.Body.DeserializeJson<LoginResponse>().Token);
                })
                .Result;
        }

        private void Channel_creation_successful()
        {
            Assert.Equal(HttpStatusCode.OK, createChannelResult.StatusCode);
        }

        private void User_tries_to_create_new_channel_providing_channel_name_that_already_exists()
        {
            createNewChannelResult = config.Browser.Post("/channel/", with =>
                {
                    with.BodyJson(new CreateChannelRequest
                    {
                        ChannelName = "Developer",
                        UserId = loginResult.Body.DeserializeJson<LoginResponse>().User.Id,
                        TeamId = team.Id
                    });
                    with.Accept(new MediaRange("application/json"));
                    with.Header("Authorization", loginResult.BodyJson<LoginResponse>().Token);
                })
                .Result;
        }

        private void Channel_creation_unsuccessful()
        {
            Assert.Equal(HttpStatusCode.BadRequest, createNewChannelResult.StatusCode);
            var response = createNewChannelResult.BodyJson<Error>();
            Assert.Equal("Channel with that name already exists", response.Message);
        }
    }
}
