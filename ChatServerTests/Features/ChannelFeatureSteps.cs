using System;
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
        private readonly FeatureHelper helper;
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

            helper = new FeatureHelper(config);
            
        }

        #endregion

        private async Task Given_the_user_is_logged_in()
        {
            registerResult = await helper.RegisterResponse(user);

            loginResult = await helper.LoginResponse(user);

            Assert.Equal(HttpStatusCode.OK, loginResult.StatusCode);
            var body = loginResult.BodyJson<LoginResponse>();
            Assert.Equal(body.User.Username, user.Username);
            Assert.NotNull(body.Token);
            Assert.NotEmpty(body.Token);

        }

        private async Task Given_the_user_creates_team_and_is_admin()
        {
            team = DataGenerator.GenerateSingleTeam(config.Context);

            createTeamResult = await helper.CreateTeamResponse(team, loginResult.BodyJson<LoginResponse>().Token);

            role = DataGenerator.GenerateSigleRole(config.Context, "Admin");
            createRoleResult = await helper.CreateRoleResponse(role, loginResult.BodyJson<LoginResponse>().Token);

            assignRoleResult = await helper.AssignRoleResponse(createRoleResult.BodyJson<Role>().Id,
                createTeamResult.BodyJson<Team>().Id, loginResult.BodyJson<LoginResponse>().User.Id,
                loginResult.BodyJson<LoginResponse>().Token);
        }


        private async Task User_tries_to_create_new_channel_providing_channel_name()
        {
            var channel = DataGenerator.GenerateSingleChannel(config.Context, createTeamResult.BodyJson<Team>().Id);
            
            createChannelResult = await helper.CreateChannelResponse(channel,
                loginResult.BodyJson<LoginResponse>().User.Id,
                loginResult.BodyJson<LoginResponse>().Token);
        }
       
        private Task Channel_creation_successful()
        {
            //Assert.Equal("dsa", createChannelResult.BodyJson<Msg>().Message);
            Assert.Equal(HttpStatusCode.OK, createChannelResult.StatusCode);
            return Task.CompletedTask;
        }

        private async Task User_tries_to_create_new_channel_providing_channel_name_that_already_exists()
        {
            createNewChannelResult = await helper.CreateChannelResponse(createChannelResult.BodyJson<Channel>(),
                loginResult.Body.DeserializeJson<LoginResponse>().User.Id, loginResult.BodyJson<LoginResponse>().Token);
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
                createNewChannelResult = await helper.CreateChannelResponse(c,
                    loginResult.Body.DeserializeJson<LoginResponse>().User.Id,
                    loginResult.BodyJson<LoginResponse>().Token);
            }

            role = DataGenerator.GenerateSigleRole(config.Context, "AnyRole");

            userRole = DataGenerator.GenerateSigleUserRole(config.Context, team.Id, user.Id, role.Id);
        }

        private async Task Users_wants_to_see_list_of_all_channels_inside_of_that_team()
        {
            retrievedChannelListResult = await helper.RetrieveChannelListResponse(createTeamResult.BodyJson<Team>().Id,
                loginResult.BodyJson<LoginResponse>().Token);

        }

        private Task List_retrieved_successfully()
        {
            Assert.Equal(channels.Count, retrievedChannelListResult.BodyJson<List<Channel>>().Count);
            return Task.CompletedTask;
        }

        private async Task Users_wants_to_change_channel_name()
        {
            channel = DataGenerator.GenerateSingleChannel(config.Context, createTeamResult.BodyJson<Team>().Id);

            createNewChannelResult = await helper.CreateChannelResponse(channel,
                loginResult.Body.DeserializeJson<LoginResponse>().User.Id,
                loginResult.Body.DeserializeJson<LoginResponse>().Token);


            editChannelNameResult = await helper.EditChannelResponse(createNewChannelResult.BodyJson<Channel>().Id,
                "Developer", loginResult.Body.DeserializeJson<LoginResponse>().Token);

        }

        private Task Channel_name_change_successful()
        {
            Assert.Equal("Developer", editChannelNameResult.BodyJson<Channel>().ChannelName);
            return Task.CompletedTask;
        }

        private async Task Users_tries_to_delete_that_channel()
        {
            deleteChannelResult = await helper.DeleteChannelResponse(createChannelResult.BodyJson<Channel>().Id,
                loginResult.Body.DeserializeJson<LoginResponse>().Token);
        }

        private Task Channel_deleteion_successful()
        {
            Assert.Equal("Channel Deleted", deleteChannelResult.BodyJson<Msg>().Message);
            return Task.CompletedTask;
        }

        private async Task Users_tries_to_retrieve_created_channel()
        {
            getChannelResult = await helper.GetChannelResponse(createChannelResult.BodyJson<Channel>().Id,
                loginResult.Body.DeserializeJson<LoginResponse>().Token);
        }

        private Task Channel_retrieved_successfully()
        {
            Assert.Equal(createChannelResult.BodyJson<Channel>().Id, getChannelResult.BodyJson<Channel>().Id);
            return Task.CompletedTask;
        }

        private async Task Given_there_are_several_roles_in_database()
        {
            role = DataGenerator.GenerateSigleRole(config.Context, "Admin");
            createRoleResult = await helper.CreateRoleResponse(role, loginResult.BodyJson<LoginResponse>().Token);

            role = DataGenerator.GenerateSigleRole(config.Context, "Dev");
            createRoleResult = await helper.CreateRoleResponse(role, loginResult.BodyJson<LoginResponse>().Token);
        }

        private async Task User_tries_to_create_new_channel_providing_channel_name_with_non_admin_account()
        {
            team = DataGenerator.GenerateSingleTeam(config.Context);


            createTeamResult = await helper.CreateTeamResponse(team, loginResult.BodyJson<LoginResponse>().Token);



            var user2 = DataGenerator.GenerateSingleUser(config.Context);

            registerResult = await helper.RegisterResponse(user2);

            assignRoleResult = await helper.AssignRoleResponse(createRoleResult.BodyJson<Role>().Id,
                createTeamResult.BodyJson<Team>().Id, registerResult.BodyJson<User>().Id,
                loginResult.BodyJson<LoginResponse>().Token);
            

            var channel = DataGenerator.GenerateSingleChannel(config.Context, createTeamResult.BodyJson<Team>().Id);
            createChannelResult = await helper.CreateChannelResponse(channel, registerResult.BodyJson<User>().Id,
                loginResult.BodyJson<LoginResponse>().Token);

        }

        private Task Channel_creation_failed()
        {
            Assert.Equal("You are not admin!", createChannelResult.BodyJson<Msg>().Message);
            return Task.CompletedTask;
        }

        private async Task Users_wants_to_change_channel_name_providing_channel_name_that_already_exists()
        {
            editChannelNameResult = await helper.EditChannelResponse(createChannelResult.BodyJson<Channel>().Id,
                createChannelResult.BodyJson<Channel>().ChannelName,
                loginResult.BodyJson<LoginResponse>().Token);
        }

        private Task Channel_name_change_unccessful()
        {
            Assert.Equal("Channel with that name already exists", editChannelNameResult.BodyJson<Msg>().Message);
            return Task.CompletedTask;
        }
    }
}
