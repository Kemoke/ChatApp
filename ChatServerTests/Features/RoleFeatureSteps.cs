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
using Xunit;
using Xunit.Abstractions;

namespace ChatServerTests.Features
{
    public partial class RoleFeature : FeatureFixture
    {
        #region Setup/Teardown

        private readonly FeaturesConfig config;
        private BrowserResponse loginResult;
        private BrowserResponse createTeamResult;
        private BrowserResponse createRoleResult;
        private BrowserResponse deleteRoleResult;
        private BrowserResponse unsignRoleResult;
        private BrowserResponse assignRoleResult;
        private BrowserResponse editRoleResult;
        private BrowserResponse getRoleResult;
        private BrowserResponse getRoleListResult;
        private BrowserResponse createRolesResult;
        private List<Role> roleList;
        private readonly User user;
        private readonly Team team;
        private Role role;

        public RoleFeature(ITestOutputHelper output) : base(output)
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

        private async Task Role_is_being_created()
        {
            role = DataGenerator.GenerateSigleRole(config.Context, "Developer");
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

        private async Task Role_is_then_deleted()
        {
            deleteRoleResult = await config.Browser.Delete("/role/" + createRoleResult.BodyJson<Role>().Id, with =>
            {
                with.Accept(new MediaRange("application/json"));
                with.Header("Authorization", loginResult.BodyJson<LoginResponse>().Token);
            });
        }

        private Task Role_deletion_successful()
        {
            Assert.Equal("Role Deleted", deleteRoleResult.BodyJson<Msg>().Message);
            return Task.CompletedTask;
        }

        private async Task Role_is_then_edited()
        {
            editRoleResult = await config.Browser.Put("/role/" + createRoleResult.BodyJson<Role>().Id, with =>
            {
                with.BodyJson(new EditRoleRequest
                {
                    RoleName = "Hello"
                });
                with.Accept(new MediaRange("application/json"));
                with.Header("Authorization", loginResult.BodyJson<LoginResponse>().Token);
            });
        }

        private Task Role_edit_successful()
        {
            Assert.Equal("Hello", editRoleResult.BodyJson<Role>().Name);
            return Task.CompletedTask;
        }

        private async Task Role_is_being_requested_by_id()
        {
            getRoleResult = await config.Browser.Get("/role/" + createRoleResult.BodyJson<Role>().Id, with =>
            {
                with.Accept(new MediaRange("application/json"));
                with.Header("Authorization", loginResult.BodyJson<LoginResponse>().Token);
            });
        }

        private Task Role_request_successful()
        {
            Assert.Equal(role.Name, getRoleResult.BodyJson<Role>().Name);
            return Task.CompletedTask;
        }

        private async Task Given_that_several_roles_exist_in_database()
        {
            roleList = DataGenerator.GenerateRoleList(config.Context, 10, "Dev").ToList();

            foreach (var r in roleList)
            {
                createRolesResult = await config.Browser.Post("/role/", with =>
                {
                    with.BodyJson(new CreateRoleRequest
                    {
                        Name = r.Name
                    });
                    with.Accept(new MediaRange("application/json"));
                    with.Header("Authorization", loginResult.BodyJson<LoginResponse>().Token);
                });
            }
            
        }

        private async Task User_requests_list_of_roles()
        {
            getRoleListResult = await config.Browser.Get("/role/", with =>
            {
                with.Accept(new MediaRange("application/json"));
                with.Header("Authorization", loginResult.BodyJson<LoginResponse>().Token);
            });
        }

        private Task Role_list_successfully_retrieved()
        {
            Assert.NotEmpty(getRoleListResult.BodyJson<List<Role>>());
            return Task.CompletedTask;
        }

        private async Task Given_that_team_exists_in_database()
        {
            createTeamResult = await config.Browser.Post("/team/", with =>
            {
                with.BodyJson(new CreateTeamRequest
                {
                    Name = team.Name
                });
                with.Accept(new MediaRange("application/json"));
                with.Header("Authorization", loginResult.BodyJson<LoginResponse>().Token);
            });
        }

        private async Task Role_is_assigned_to_a_user_belonging_to_a_certain_team()
        {
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

        private Task Role_assignment_successful()
        {
            Assert.Equal(loginResult.BodyJson<LoginResponse>().User.Id, assignRoleResult.BodyJson<UserTeam>().UserId);
            Assert.Equal(createRoleResult.BodyJson<Role>().Id, assignRoleResult.BodyJson<UserTeam>().RoleId);
            Assert.Equal(createTeamResult.BodyJson<Team>().Id, assignRoleResult.BodyJson<UserTeam>().TeamId);
            return Task.CompletedTask;
        }

        private async Task Users_role_is_unassigned()
        {
            unsignRoleResult = await config.Browser.Delete("/role/unsign", with =>
            {
                with.BodyJson(new UnsignRoleRequest
                {
                    TeamId = assignRoleResult.BodyJson<UserTeam>().TeamId,
                    RoleId = assignRoleResult.BodyJson<UserTeam>().RoleId,
                    UserId = assignRoleResult.BodyJson<UserTeam>().UserId

                });
                with.Accept(new MediaRange("application/json"));
                with.Header("Authorization", loginResult.BodyJson<LoginResponse>().Token);
            });
        }

        private Task Role_unassign_successful()
        {
            StepExecution.Current.Comment(unsignRoleResult.BodyJson<Msg>().Message);
            Assert.Equal("Role Unassigned", unsignRoleResult.BodyJson<Msg>().Message);
            return Task.CompletedTask;
        }
    }
}
