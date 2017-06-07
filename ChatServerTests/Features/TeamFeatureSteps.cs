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
        private readonly User user;
        private readonly Team team;
        private readonly FeaturesConfig config;
        private BrowserResponse result;
        private BrowserResponse loginResult;
        private BrowserResponse teamListResponse;
        private BrowserResponse teamInfoResponse;
        private BrowserResponse editTeamInfoResponse;
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
        }

        #endregion

        private async Task Given_the_user_is_logged_in()
        {
            loginResult = await config.Browser.Post("/auth/register", with =>
            {
                with.BodyJson(new RegisterRequest {User = user});
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

        private async Task User_tries_to_create_new_team_providing_team_name()
        {
            result = await config.Browser.Post("/team/", with =>
            {
                with.BodyJson(new CreateTeamRequest
                {
                    Name = team.Name
                    
                });
                with.Accept(new MediaRange("application/json"));
                with.Header("Authorization", loginResult.BodyJson<LoginResponse>().Token);
            });
        }

        private Task Team_creation_successful()
        {
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            return Task.CompletedTask;
        }

        private async Task Given_there_exists_a_team_in_the_database()
        {
            result = await config.Browser.Post("/team/", with =>
            {
                with.BodyJson(new CreateTeamRequest
                {
                    Name = team.Name

                });
                with.Accept(new MediaRange("application/json"));
                with.Header("Authorization", loginResult.BodyJson<LoginResponse>().Token);
            });
        }

        private Task Team_creation_unsuccessful()
        {
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            var response = result.BodyJson<Msg>();
            Assert.Equal("Channel with that name already exists", response.Message);
            return Task.CompletedTask;
        }

        private async Task User_tries_to_retrieve_list_of_teams_in_which_he_is_in()
        {
            teamListResponse = await config.Browser.Get("/team/", with =>
            {
                with.Accept(new MediaRange("application/json"));
                with.Header("Authorization", loginResult.BodyJson<LoginResponse>().Token);
            });
        }

        private Task List_retrieved_successfully()
        {
            Assert.NotEmpty(teamListResponse.BodyJson<List<Team>>());
            return Task.CompletedTask;
        }

        private async Task User_tries_to_retrieve_about_the_team()
        {
            teamInfoResponse = await config.Browser.Get("/team/"+ result.BodyJson<Team>().Id, with =>
            {
                with.Accept(new MediaRange("application/json"));
                with.Header("Authorization", loginResult.BodyJson<LoginResponse>().Token);
            });
        }

        private Task Info_retrieved_successfully()
        {
            Assert.Equal(result.BodyJson<Team>().Id, teamInfoResponse.BodyJson<Team>().Id);
            return Task.CompletedTask;
        }

        private async Task User_tries_to_edit_team_info()
        {
            editTeamInfoResponse = await config.Browser.Put("/team/" + result.BodyJson<Team>().Id, with =>
            {
                with.BodyJson(new Team
                {
                    Name = "IUS"
                });
                with.Accept(new MediaRange("application/json"));
                with.Header("Authorization", loginResult.BodyJson<LoginResponse>().Token);
            });
        }

        private Task Info_edited_successfully()
        {
            Assert.Equal("IUS", editTeamInfoResponse.BodyJson<Team>().Name);
            return Task.CompletedTask;
        }

        private async Task Given_another_user_is_registered()
        {
            var user2 = DataGenerator.GenerateSingleUser(config.Context);

            registerResult = await config.Browser.Post("/auth/register", with =>
            {
                with.BodyJson(new RegisterRequest { User = user2 });
                with.Accept(new MediaRange("application/json"));
            });
        }

        private async Task Role_is_being_created()
        {
            var role = DataGenerator.GenerateSigleRole(config.Context, "Developer");

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

        private async Task User_tries_to_add_other_user_to_the_team()
        {
            addUserResult = await config.Browser.Post("/team/user/add", with =>
            {
                with.BodyJson(new AssignRoleRequest
                {
                    TeamId = result.BodyJson<Team>().Id,
                    RoleId = createRoleResult.BodyJson<Role>().Id,
                    UserId = registerResult.BodyJson<User>().Id

                });
                with.Accept(new MediaRange("application/json"));
                with.Header("Authorization", loginResult.BodyJson<LoginResponse>().Token);
            });
        }

        private Task Adding_user_successful()
        {
            Assert.Equal(registerResult.BodyJson<User>().Id, addUserResult.BodyJson<UserTeam>().UserId);
            Assert.Equal(createRoleResult.BodyJson<Role>().Id, addUserResult.BodyJson<UserTeam>().RoleId);
            Assert.Equal(result.BodyJson<Team>().Id, addUserResult.BodyJson<UserTeam>().TeamId);
            return Task.CompletedTask;
        }

        private async Task User_tries_to_remove_another_user_from_team()
        {
            removeUserResult = await config.Browser.Delete("/team/user/remove", with =>
            {
                with.BodyJson(new UnsignRoleRequest
                {
                    TeamId = addUserResult.BodyJson<UserTeam>().TeamId,
                    RoleId = addUserResult.BodyJson<UserTeam>().RoleId,
                    UserId = addUserResult.BodyJson<UserTeam>().UserId

                });
                with.Accept(new MediaRange("application/json"));
                with.Header("Authorization", loginResult.BodyJson<LoginResponse>().Token);
            });
        }

        private Task Remove_user_successful()
        {
            Assert.Equal("User Removed", removeUserResult.BodyJson<Msg>().Message);
            return Task.CompletedTask;
        }

        private async Task User_tries_to_delete_team()
        {
            deleteTeamResult = await config.Browser.Delete("/team/" + result.BodyJson<Team>().Id, with =>
            {
                with.Accept(new MediaRange("application/json"));
                with.Header("Authorization", loginResult.BodyJson<LoginResponse>().Token);
            });
        }

        private Task Team_delete_successfully()
        {
            Assert.Equal("Deleted", deleteTeamResult.BodyJson<Msg>().Message);
            return Task.CompletedTask;
        }
    }
}