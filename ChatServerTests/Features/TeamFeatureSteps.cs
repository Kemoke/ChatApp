using System.Collections.Generic;
using System.Threading.Tasks;
using ChatServer.Model;
using ChatServer.Request;
using ChatServer.Response;
using LightBDD.XUnit2;
using Nancy;
using Nancy.Responses.Negotiation;
using Nancy.Testing;
using Xunit;
using Xunit.Abstractions;

namespace ChatServerTests.Features
{
    public partial class TeamFeature : FeatureFixture
    {
        private readonly FeatureHelper helper;
        private readonly User user;
        private readonly Team team;
        private readonly FeaturesConfig config;
        private BrowserResponse createTeamResult;
        private BrowserResponse loginResult;
        private BrowserResponse teamListResult;
        private BrowserResponse teamInfoResult;
        private BrowserResponse editTeamInfoResult;
        private BrowserResponse registerResult;
        private BrowserResponse createRoleResult;
        private BrowserResponse addUserResult;
        private BrowserResponse removeUserResult;
        private BrowserResponse deleteTeamResult;



        #region Setup/Teardown

        public TeamFeature(ITestOutputHelper output) : base(output)
        {

            config = new FeaturesConfig();
            
            user = DataGenerator.GenerateSingleUser(config.Context);
            
            team = DataGenerator.GenerateSingleTeam(config.Context);

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

        private async Task User_tries_to_create_new_team_providing_team_name()
        {
            createTeamResult = await helper.CreateTeamResponse(team, loginResult.BodyJson<LoginResponse>().Token);
        }

        private Task Team_creation_successful()
        {
            Assert.Equal(HttpStatusCode.OK, createTeamResult.StatusCode);
            return Task.CompletedTask;
        }

        private async Task Given_there_exists_a_team_in_the_database()
        {
            createTeamResult = await helper.CreateTeamResponse(team, loginResult.BodyJson<LoginResponse>().Token);
        }

        private Task Team_creation_unsuccessful()
        {
            Assert.Equal(HttpStatusCode.BadRequest, createTeamResult.StatusCode);
            var response = createTeamResult.BodyJson<Msg>();
            Assert.Equal("Channel with that name already exists", response.Message);
            return Task.CompletedTask;
        }

        private async Task User_tries_to_retrieve_list_of_teams_in_which_he_is_in()
        {
            teamListResult = await helper.GetTeamListResponse(loginResult.BodyJson<LoginResponse>().Token);
        }

        private Task List_retrieved_successfully()
        {
            Assert.NotEmpty(teamListResult.BodyJson<List<Team>>());
            return Task.CompletedTask;
        }

        private async Task User_tries_to_retrieve_about_the_team()
        {
            teamInfoResult = await helper.GetTeamInfoResponse(createTeamResult.BodyJson<Team>().Id,
                loginResult.BodyJson<LoginResponse>().Token);
        }

        private Task Info_retrieved_successfully()
        {
            Assert.Equal(createTeamResult.BodyJson<Team>().Id, teamInfoResult.BodyJson<Team>().Id);
            return Task.CompletedTask;
        }

        private async Task User_tries_to_edit_team_info()
        {
            editTeamInfoResult = await helper.EditTeamInfoResponse(createTeamResult.BodyJson<Team>().Id,
                loginResult.BodyJson<LoginResponse>().Token);
        }

        private Task Info_edited_successfully()
        {
            Assert.Equal("IUS", editTeamInfoResult.BodyJson<Team>().Name);
            return Task.CompletedTask;
        }

        private async Task Given_another_user_is_registered()
        {
            var user2 = DataGenerator.GenerateSingleUser(config.Context);

            registerResult = await helper.RegisterResponse(user2);
        }

        private async Task Role_is_being_created()
        {
            var role = DataGenerator.GenerateSigleRole(config.Context, "Developer");

            createRoleResult = await helper.CreateRoleResponse(role, loginResult.BodyJson<LoginResponse>().Token);
        }

        private async Task User_tries_to_add_other_user_to_the_team()
        {
            addUserResult = await helper.AddUserResponse(createTeamResult.BodyJson<Team>().Id,
                createRoleResult.BodyJson<Role>().Id, registerResult.BodyJson<User>().Id,
                loginResult.BodyJson<LoginResponse>().Token);
        }

        private Task Adding_user_successful()
        {
            Assert.Equal(registerResult.BodyJson<User>().Id, addUserResult.BodyJson<UserTeam>().UserId);
            Assert.Equal(createRoleResult.BodyJson<Role>().Id, addUserResult.BodyJson<UserTeam>().RoleId);
            Assert.Equal(createTeamResult.BodyJson<Team>().Id, addUserResult.BodyJson<UserTeam>().TeamId);
            return Task.CompletedTask;
        }

        private async Task User_tries_to_remove_another_user_from_team()
        {
            removeUserResult = await helper.RemoveUserResponse(addUserResult.BodyJson<UserTeam>().TeamId,
                addUserResult.BodyJson<UserTeam>().RoleId, addUserResult.BodyJson<UserTeam>().UserId,
                loginResult.BodyJson<LoginResponse>().Token);
        }

        private Task Remove_user_successful()
        {
            Assert.Equal("User Removed", removeUserResult.BodyJson<Msg>().Message);
            return Task.CompletedTask;
        }

        private async Task User_tries_to_delete_team()
        {
            deleteTeamResult = await helper.DeleteTeamResponse(createTeamResult.BodyJson<Team>().Id,
                loginResult.BodyJson<LoginResponse>().Token);
        }

        private Task Team_delete_successfully()
        {
            Assert.Equal("Deleted", deleteTeamResult.BodyJson<Msg>().Message);
            return Task.CompletedTask;
        }
    }
}