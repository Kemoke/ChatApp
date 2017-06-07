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
    public partial class Secure_Module_Feature : FeatureFixture
    {
        private BrowserResponse loginResult;
        private BrowserResponse unsignRoleResult;
        private readonly FeaturesConfig config;
        private readonly User user;
        #region Setup/Teardown
        public Secure_Module_Feature(ITestOutputHelper output) : base(output)
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

        private void User_tries_to_perform_request_without_auhorization_header()
        {
            unsignRoleResult = config.Browser.Delete("/role/unsign", with =>
            {
                with.BodyJson(new UnsignRoleRequest
                {
                    TeamId = 1,
                    RoleId = 2,
                    UserId = 3
                });
                with.Accept(new MediaRange("application/json"));
                with.Header("Authorization","");
            }).Result;
        }

        private void Request_failed()
        {
            Assert.Equal("Not authorized", unsignRoleResult.BodyJson<Msg>().Message);
        }
    }
}