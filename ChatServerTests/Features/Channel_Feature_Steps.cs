using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChatServer.Model;
using ChatServer.Request;
using ChatServer.Response;
using LightBDD.Framework;
using LightBDD.Framework.Commenting;
using LightBDD.XUnit2;
using Nancy;
using Nancy.Responses.Negotiation;
using Nancy.Testing;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace ChatServerTests.Features
{
    public partial class Channel_Feature : FeatureFixture
    {
        private readonly User user;
        private Team team;
        private Role role;
        private UserTeam userRole;
        private Channel channel;
        private List<Channel> channels;
        private readonly FeaturesConfig config;
        private BrowserResponse createTeamResult;
        private BrowserResponse addRoleResult;
        private BrowserResponse editChannelNameResult;
        private BrowserResponse retrievedChannelListResult;
        private BrowserResponse createChannelResult;
        private BrowserResponse createNewChannelResult;
        private BrowserResponse loginResult;



        #region Setup/Teardown

        public Channel_Feature(ITestOutputHelper output) : base(output)
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
            createTeamResult = config.Browser.Post("/team/", with =>
            {
                with.BodyJson(new CreateTeamRequest
                {
                    Name = team.Name
                });
                with.Accept(new MediaRange("application/json"));
                with.Header("Authorization", loginResult.BodyJson<LoginResponse>().Token);
            }).Result;

            role = DataGenerator.GenerateSigleRole(config.Context, "Admin");

            userRole = DataGenerator.GenerateSigleUserRole(config.Context, team.Id, user.Id, role.Id);

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

        private void Given_the_user_is_inside_of_a_team()
        {
            channels = DataGenerator.GenerateChannelList(config.Context, team.Id, 10).ToList();

            foreach (var c in channels)
            {
                createNewChannelResult = config.Browser.Post("/channel/", with =>
                    {
                        with.BodyJson(new CreateChannelRequest
                        {
                            ChannelName = c.ChannelName,
                            UserId = loginResult.Body.DeserializeJson<LoginResponse>().User.Id,
                            TeamId = team.Id
                        });
                        with.Accept(new MediaRange("application/json"));
                        with.Header("Authorization", loginResult.BodyJson<LoginResponse>().Token);
                    })
                    .Result;
            }

            role = DataGenerator.GenerateSigleRole(config.Context, "AnyRole");

            userRole = DataGenerator.GenerateSigleUserRole(config.Context, team.Id, user.Id, role.Id);
        }

        private void Users_wants_to_see_list_of_all_channels_inside_of_that_team()
        {
            retrievedChannelListResult = config.Browser.Get("/channel/", with =>
                {
                    with.BodyJson(new ListChannelRequest()
                    {
                        TeamId = team.Id
                    });
                    with.Accept(new MediaRange("application/json"));
                    with.Header("Authorization", loginResult.BodyJson<LoginResponse>().Token);
                })
                .Result;
        }

        private void List_retrieved_successfully()
        {
            Assert.Equal(channels.Count, retrievedChannelListResult.BodyJson<List<Channel>>().Count);
        }

        private void Users_wants_to_change_channel_name()
        {
            channel = DataGenerator.GenerateSingleChannel(config.Context, team.Id);

            createNewChannelResult = config.Browser.Post("/channel/", with =>
                {
                    with.BodyJson(new CreateChannelRequest
                    {
                        ChannelName = channel.ChannelName,
                        UserId = loginResult.Body.DeserializeJson<LoginResponse>().User.Id,
                        TeamId = channel.TeamId
                    });
                    with.Accept(new MediaRange("application/json"));
                    with.Header("Authorization", loginResult.BodyJson<LoginResponse>().Token);
                })
                .Result;

            editChannelNameResult = config.Browser.Put("/channel/"+createNewChannelResult.BodyJson<Channel>().Id, with =>
                {
                    with.BodyJson(new Channel
                    {
                        ChannelName = "Developer"
                    });
                    with.Accept(new MediaRange("application/json"));
                    with.Header("Authorization", loginResult.BodyJson<LoginResponse>().Token);
                })
                .Result;
        }

        private void Channel_name_change_successful()
        {
            StepExecution.Current.Comment(editChannelNameResult.BodyJson<Error>().Message);
            Assert.Equal("Developer", editChannelNameResult.BodyJson<Channel>().ChannelName);
        }
    }
}
