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
    public partial class SecureModuleFeature : FeatureFixture
    {
        private readonly ITestOutputHelper output;
        private BrowserResponse loginResult;
        private BrowserResponse unsignRoleResult;
        private readonly FeaturesConfig config;
        private readonly User user;
        #region Setup/Teardown
        public SecureModuleFeature(ITestOutputHelper output) : base(output)
        {
            this.output = output;

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

        private async Task User_tries_to_perform_request_without_auhorization_header()
        {
            unsignRoleResult = await config.Browser.Delete("/role/unsign", with =>
            {
                with.BodyJson(new UnsignRoleRequest
                {
                    TeamId = 1,
                    RoleId = 2,
                    UserId = 3
                });
                with.Accept(new MediaRange("application/json"));
                with.Header("Authorization","");
            });
        }

        private Task Request_failed()
        {
            Assert.Equal(HttpStatusCode.Unauthorized, unsignRoleResult.StatusCode);
            Assert.Equal("Not authorized", unsignRoleResult.BodyJson<Msg>().Message);
            return Task.CompletedTask;
        }
    }
}