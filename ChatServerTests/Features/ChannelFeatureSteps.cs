﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public partial class ChannelFeature : FeatureFixture
    {
        private readonly User user;
        private Team team;
        private Role role;
        private UserTeam userRole;
        private Channel channel;
        private List<Channel> channels;
        private readonly FeaturesConfig config;
        private BrowserResponse createTeamResult;
        private BrowserResponse deleteChannelResult;
        private BrowserResponse addRoleResult;
        private BrowserResponse editChannelNameResult;
        private BrowserResponse retrievedChannelListResult;
        private BrowserResponse createChannelResult;
        private BrowserResponse createNewChannelResult;
        private BrowserResponse loginResult;
        private BrowserResponse getChannelResult;
        private BrowserResponse registerResult;
        private BrowserResponse assignRoleResult;
        private BrowserResponse createRoleResult;



        #region Setup/Teardown

        public ChannelFeature(ITestOutputHelper output) : base(output)
        {

            config = new FeaturesConfig();

            user = DataGenerator.GenerateSingleUser(config.Context);

            
        }

        #endregion

        private async Task Given_the_user_is_logged_in()
        {
            loginResult = await config.Browser.Post("/auth/register", with =>
            {
                with.BodyJson(new RegisterRequest { User = user });
                with.Accept(new MediaRange("application/json"));
            });

            loginResult = await config.Browser.Post("/auth/login", with =>
            {
                with.BodyJson(new LoginRequest
                {
                    Username = user.Username,
                    Password = user.Password
                });
                with.Accept(new MediaRange("application/json"));
            });

            Assert.Equal(HttpStatusCode.OK, loginResult.StatusCode);
            var body = loginResult.BodyJson<LoginResponse>();
            Assert.Equal(body.User.Username, user.Username);
            Assert.NotNull(body.Token);
            Assert.NotEmpty(body.Token);

        }

        private async Task Given_the_user_creates_team_and_is_admin()
        {
            team = DataGenerator.GenerateSingleTeam(config.Context);

            createTeamResult = await config.Browser.Post("/team/", with =>
            {
                with.BodyJson(new CreateTeamRequest
                {
                    Name = team.Name
                });
                with.Accept(new MediaRange("application/json"));
                with.Header("Authorization", loginResult.BodyJson<LoginResponse>().Token);
            });

            role = DataGenerator.GenerateSigleRole(config.Context, "Admin");
            createRoleResult = await config.Browser.Post("/role/", with =>
            {
                with.BodyJson(new CreateRoleRequest
                {
                    Name = role.Name
                });
                with.Accept(new MediaRange("application/json"));
                with.Header("Authorization", loginResult.BodyJson<LoginResponse>().Token);
            });

            assignRoleResult = await config.Browser.Post("/role/assign", with =>
            {
                with.BodyJson(new AssignRoleRequest
                {
                    TeamId = createTeamResult.BodyJson<Team>().Id,
                    RoleId = createRoleResult.BodyJson<Role>().Id,
                    UserId = loginResult.BodyJson<LoginResponse>().User.Id

                });
                with.Accept(new MediaRange("application/json"));
                with.Header("Authorization", loginResult.BodyJson<LoginResponse>().Token);
            });
        }

        private async Task User_tries_to_create_new_channel_providing_channel_name()
        {
            var channel = DataGenerator.GenerateSingleChannel(config.Context, createTeamResult.BodyJson<Team>().Id);
            createChannelResult = await config.Browser.Post("/channel/", with =>
                {
                    with.BodyJson(new CreateChannelRequest
                    {
                        ChannelName = channel.ChannelName,
                        UserId = loginResult.Body.DeserializeJson<LoginResponse>().User.Id,
                        TeamId = channel.TeamId
                    });
                    with.Accept(new MediaRange("application/json"));
                    with.Header("Authorization", loginResult.Body.DeserializeJson<LoginResponse>().Token);
                });
        }
       
        private Task Channel_creation_successful()
        {
            //Assert.Equal("dsa", createChannelResult.BodyJson<Msg>().Message);
            Assert.Equal(HttpStatusCode.OK, createChannelResult.StatusCode);
            return Task.CompletedTask;
        }

        private async Task User_tries_to_create_new_channel_providing_channel_name_that_already_exists()
        {
            createNewChannelResult = await config.Browser.Post("/channel/", with =>
                {
                    with.BodyJson(new CreateChannelRequest
                    {
                        ChannelName = createChannelResult.BodyJson<Channel>().ChannelName,
                        UserId = loginResult.Body.DeserializeJson<LoginResponse>().User.Id,
                        TeamId = createChannelResult.BodyJson<Channel>().TeamId
                    });
                    with.Accept(new MediaRange("application/json"));
                    with.Header("Authorization", loginResult.BodyJson<LoginResponse>().Token);
                });
        }

        private Task Channel_creation_unsuccessful()
        {
            Assert.Equal(HttpStatusCode.BadRequest, createNewChannelResult.StatusCode);
            var response = createNewChannelResult.BodyJson<Msg>();
            Assert.Equal("Channel with that name already exists", response.Message);
            return Task.CompletedTask;
        }

        private async Task Given_the_user_is_inside_of_a_team_and_there_exists_list_of_channels_in_database()
        {
            
            channels = DataGenerator.GenerateChannelList(config.Context, createTeamResult.BodyJson<Team>().Id, 10).ToList();

            foreach (var c in channels)
            {
                createNewChannelResult = await config.Browser.Post("/channel/", with =>
                    {
                        with.BodyJson(new CreateChannelRequest
                        {
                            ChannelName = c.ChannelName,
                            UserId = loginResult.Body.DeserializeJson<LoginResponse>().User.Id,
                            TeamId = createTeamResult.BodyJson<Team>().Id
                        });
                        with.Accept(new MediaRange("application/json"));
                        with.Header("Authorization", loginResult.BodyJson<LoginResponse>().Token);
                    });
            }

            role = DataGenerator.GenerateSigleRole(config.Context, "AnyRole");

            userRole = DataGenerator.GenerateSigleUserRole(config.Context, team.Id, user.Id, role.Id);
        }

        private async Task Users_wants_to_see_list_of_all_channels_inside_of_that_team()
        {
            retrievedChannelListResult = await config.Browser.Get("/channel/", with =>
                {
                    with.BodyJson(new ListChannelRequest
                    {
                        TeamId = createTeamResult.BodyJson<Team>().Id
                    });
                    with.Accept(new MediaRange("application/json"));
                    with.Header("Authorization", loginResult.BodyJson<LoginResponse>().Token);
                });
        }

        private Task List_retrieved_successfully()
        {
            Assert.Equal(channels.Count, retrievedChannelListResult.BodyJson<List<Channel>>().Count);
            return Task.CompletedTask;
        }

        private async Task Users_wants_to_change_channel_name()
        {
            channel = DataGenerator.GenerateSingleChannel(config.Context, team.Id);

            createNewChannelResult = await config.Browser.Post("/channel/", with =>
                {
                    with.BodyJson(new CreateChannelRequest
                    {
                        ChannelName = channel.ChannelName,
                        UserId = loginResult.Body.DeserializeJson<LoginResponse>().User.Id,
                        TeamId = createTeamResult.BodyJson<Team>().Id
                    });
                    with.Accept(new MediaRange("application/json"));
                    with.Header("Authorization", loginResult.BodyJson<LoginResponse>().Token);
                });

            editChannelNameResult = await config.Browser.Put("/channel/"+createNewChannelResult.BodyJson<Channel>().Id, with =>
                {
                    with.BodyJson(new Channel
                    {
                        ChannelName = "Developer"
                    });
                    with.Accept(new MediaRange("application/json"));
                    with.Header("Authorization", loginResult.BodyJson<LoginResponse>().Token);
                });
        }

        private Task Channel_name_change_successful()
        {
            Assert.Equal("Developer", editChannelNameResult.BodyJson<Channel>().ChannelName);
            return Task.CompletedTask;
        }

        private async Task Users_tries_to_delete_that_channel()
        {
            deleteChannelResult = await config.Browser.Delete("/channel/" + createChannelResult.BodyJson<Channel>().Id, with =>
                {
                    with.Accept(new MediaRange("application/json"));
                    with.Header("Authorization", loginResult.BodyJson<LoginResponse>().Token);
                });
        }

        private Task Channel_deleteion_successful()
        {
            Assert.Equal("Channel Deleted", deleteChannelResult.BodyJson<Msg>().Message);
            return Task.CompletedTask;
        }

        private async Task Users_tries_to_retrieve_created_channel()
        {
            getChannelResult = await config.Browser.Get("/channel/" + createChannelResult.BodyJson<Channel>().Id, with =>
                {
                    with.Accept(new MediaRange("application/json"));
                    with.Header("Authorization", loginResult.Body.DeserializeJson<LoginResponse>().Token);
                });
        }

        private Task Channel_retrieved_successfully()
        {
            Assert.Equal(createChannelResult.BodyJson<Channel>().Id, getChannelResult.BodyJson<Channel>().Id);
            return Task.CompletedTask;
        }

        private async Task Given_there_are_several_roles_in_database()
        {
            role = DataGenerator.GenerateSigleRole(config.Context, "Admin");
            createRoleResult = await config.Browser.Post("/role/", with =>
            {
                with.BodyJson(new CreateRoleRequest
                {
                    Name = role.Name
                });
                with.Accept(new MediaRange("application/json"));
                with.Header("Authorization", loginResult.BodyJson<LoginResponse>().Token);
            });

            role = DataGenerator.GenerateSigleRole(config.Context, "Dev");
            createRoleResult = await config.Browser.Post("/role/", with =>
            {
                with.BodyJson(new CreateRoleRequest
                {
                    Name = role.Name
                });
                with.Accept(new MediaRange("application/json"));
                with.Header("Authorization", loginResult.BodyJson<LoginResponse>().Token);
            });
        }

        private async Task User_tries_to_create_new_channel_providing_channel_name_with_non_admin_account()
        {
            team = DataGenerator.GenerateSingleTeam(config.Context);
            

            createTeamResult = await config.Browser.Post("/team/", with =>
            {
                with.BodyJson(new CreateTeamRequest
                {
                    Name = team.Name
                });
                with.Accept(new MediaRange("application/json"));
                with.Header("Authorization", loginResult.BodyJson<LoginResponse>().Token);
            });



            var user2 = DataGenerator.GenerateSingleUser(config.Context);

            registerResult = await config.Browser.Post("/auth/register", with =>
            {
                with.BodyJson(new RegisterRequest { User = user2 });
                with.Accept(new MediaRange("application/json"));
            });

            assignRoleResult = await config.Browser.Post("/role/assign", with =>
            {
                with.BodyJson(new AssignRoleRequest
                {
                    TeamId = createTeamResult.BodyJson<Team>().Id,
                    RoleId = createRoleResult.BodyJson<Role>().Id,
                    UserId = registerResult.BodyJson<User>().Id

                });
                with.Accept(new MediaRange("application/json"));
                with.Header("Authorization", loginResult.BodyJson<LoginResponse>().Token);
            });


            var channel = DataGenerator.GenerateSingleChannel(config.Context, createTeamResult.BodyJson<Team>().Id);
            createChannelResult = await config.Browser.Post("/channel/", with =>
                {
                    with.BodyJson(new CreateChannelRequest
                    {
                        ChannelName = channel.ChannelName,
                        UserId = registerResult.BodyJson<User>().Id,
                        TeamId = createTeamResult.BodyJson<Team>().Id
                    });
                    with.Accept(new MediaRange("application/json"));
                    with.Header("Authorization", loginResult.Body.DeserializeJson<LoginResponse>().Token);
                });
            
        }

        private Task Channel_creation_failed()
        {
            Assert.Equal("You are not admin!", createChannelResult.BodyJson<Msg>().Message);
            return Task.CompletedTask;
        }
    }
}