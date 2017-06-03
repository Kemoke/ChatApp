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
        private readonly Team team;
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

            team = DataGenerator.GenerateSingleTeam(config.Context);
        }

        #endregion

        private void Given_the_user_is_logged_in()
        {
            loginResult = config.Browser.Post("/auth/register", with =>
            {
                with.Body(JsonConvert.SerializeObject(new RegisterRequest { User = user }), "application/json");
                with.Accept(new MediaRange("application/json"));
            }).Result;
            loginResult = config.Browser.Post("/auth/login", with =>
            {
                with.Body(JsonConvert.SerializeObject(new LoginRequest
                {
                    Username = user.Username,
                    Password = user.Password
                }), "application/json");
                with.Accept(new MediaRange("application/json"));
            }).Result;
            Assert.Equal(HttpStatusCode.OK, loginResult.StatusCode);
            var body = loginResult.Body.DeserializeJson<LoginResponse>();
            Assert.Equal(body.User.Username, user.Username);
            Assert.NotNull(body.Token);
            Assert.NotEmpty(body.Token);

        }

        private void Given_the_user_creates_team_and_is_admin()
        {
           
            createTeamResult = config.Browser.Post("/team/create_team", with =>
            {
                with.Body(JsonConvert.SerializeObject(new CreateTeamRequest
                {
                    Name = team.Name

                }), "application/json");
                with.Accept(new MediaRange("application/json"));
                with.Header("Authorization", loginResult.Body.DeserializeJson<LoginResponse>().Token);
            }).Result;

            

            addRoleResult = config.Browser.Post("/role/assign_role", with =>
            {
                with.Body(JsonConvert.SerializeObject(new AssignRoleRequest
                {
                    UserId = loginResult.Body.DeserializeJson<LoginResponse>().User.Id,
                    TeamId = team.Id,
                    RoleId = 1
                }), "application/json");
                with.Accept(new MediaRange("application/json"));
                with.Header("Authorization", loginResult.Body.DeserializeJson<LoginResponse>().Token);
            })
            .Result;
            Assert.Equal(HttpStatusCode.OK,addRoleResult.StatusCode);
        }
        
        private void User_tries_to_create_new_channel_providing_channel_name()
        {
            createChannelResult = config.Browser.Post("/chat/create_channel", with =>
                {
                    with.Body(JsonConvert.SerializeObject(new CreateChannelRequest
                    {
                        ChannelName = "Developer",
                        UserId = loginResult.Body.DeserializeJson<LoginResponse>().User.Id,
                        TeamId = 1
                    }), "application/json");
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
            createNewChannelResult = config.Browser.Post("/chat/create_channel", with =>
                {
                    with.Body(JsonConvert.SerializeObject(new CreateChannelRequest
                    {
                        ChannelName = "Developer",
                        UserId = loginResult.Body.DeserializeJson<LoginResponse>().User.Id,
                        TeamId = 1
                    }), "application/json");
                    with.Accept(new MediaRange("application/json"));
                    with.Header("Authorization", loginResult.Body.DeserializeJson<LoginResponse>().Token);
                })
                .Result;
        }

        private void Channel_creation_unsuccessful()
        {
            Assert.Equal(HttpStatusCode.BadRequest, createNewChannelResult.StatusCode);
            var response = JsonConvert.DeserializeObject<Error>(createNewChannelResult.Body.AsString());
            Assert.Equal("Channel with that name already exists", response.Message);
        }
    }
}
