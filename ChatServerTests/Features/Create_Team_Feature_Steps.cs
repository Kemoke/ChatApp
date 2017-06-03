using ChatServer;
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
    public partial class Create_Team_Feature : FeatureFixture
    {
        private readonly User user;
        private readonly Team team;
        private readonly FeaturesConfig config;
        private BrowserResponse result;
        private BrowserResponse loginResult;

        

        #region Setup/Teardown

        public Create_Team_Feature(ITestOutputHelper output) : base(output)
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
                with.Body(JsonConvert.SerializeObject(new RegisterRequest {User = user}), "application/json");
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

        private void User_tries_to_create_new_team_providing_team_name()
        {
            result = config.Browser.Post("/team/create_team", with =>
            {
                with.Body(JsonConvert.SerializeObject(new CreateTeamRequest
                {
                    Name = team.Name
                    
                }), "application/json");
                with.Accept(new MediaRange("application/json"));
                with.Header("Authorization", loginResult.Body.DeserializeJson<LoginResponse>().Token);
            }).Result;
        }

        private void Team_creation_successful()
        {
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        private void Given_there_exists_a_team_in_the_database()
        {
            result = config.Browser.Post("/team/create_team", with =>
            {
                with.Body(JsonConvert.SerializeObject(new CreateTeamRequest
                {
                    Name = team.Name

                }), "application/json");
                with.Accept(new MediaRange("application/json"));
                with.Header("Authorization", loginResult.Body.DeserializeJson<LoginResponse>().Token);
            }).Result;
        }

        private void Team_creation_unsuccessful()
        {
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            var response = JsonConvert.DeserializeObject<Error>(result.Body.AsString());
            Assert.Equal("Channel with that name already exists", response.Message);
        }
    }
}