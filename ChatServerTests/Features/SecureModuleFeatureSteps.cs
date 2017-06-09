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
        private readonly FeatureHelper helper;
        private BrowserResponse registerResult;

        #region Setup/Teardown
        public SecureModuleFeature(ITestOutputHelper output) : base(output)
        {
            this.output = output;

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

        private async Task User_tries_to_perform_request_with_wrong_token()
        {
            unsignRoleResult = await helper.UnsignRoleResponse(1, 2, 3, "");
        }

        private Task Request_failed()
        {
            Assert.Equal(HttpStatusCode.Unauthorized, unsignRoleResult.StatusCode);
            Assert.Equal("Not authorized", unsignRoleResult.BodyJson<Msg>().Message);
            return Task.CompletedTask;
        }
    }
}