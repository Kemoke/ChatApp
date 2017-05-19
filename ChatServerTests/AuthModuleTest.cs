using System.Linq;
using ChatServer.Model;
using ChatServer.Request;
using ChatServer.Response;
using Nancy;
using Nancy.Json;
using Nancy.Responses.Negotiation;
using Nancy.Testing;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace ChatServerTests
{
    public class AuthModuleTest : HttpTest
    {
        private readonly ITestOutputHelper output;
        private readonly User user;
        public AuthModuleTest(ITestOutputHelper output)
        {
            this.output = output;
            user = DataGenerator.GenerateSingleUser(Context);
        }

        [Fact]
        public void TestRegister()
        {
            var result = Browser.Post("/auth/register", with =>
            {
                with.Body(JsonConvert.SerializeObject(new RegisterRequest{User = user}), "application/json");
                with.Accept(new MediaRange("application/json"));
            }).Result;
            output.WriteLine(result.Body.AsString());
            Assert.Equal(result.StatusCode, HttpStatusCode.OK);
        }

        [Fact]
        public void TestLogin()
        {
            var result = Browser.Post("/auth/register", with =>
            {
                with.Body(JsonConvert.SerializeObject(new RegisterRequest { User = user }), "application/json");
                with.Accept(new MediaRange("application/json"));
            }).Result;
            output.WriteLine(result.Body.AsString());
            result = Browser.Post("/auth/login", with =>
            {
                with.Body(JsonConvert.SerializeObject(new LoginRequest
                {
                    Username = user.Username,
                    Password = user.Password
                }), "application/json");
                with.Accept(new MediaRange("application/json"));
            }).Result;
            output.WriteLine(result.Body.AsString());
            Assert.Equal(result.StatusCode, HttpStatusCode.OK);
            var body = result.Body.DeserializeJson<LoginResponse>();
            Assert.Equal(body.User.Username, user.Username);
            Assert.NotNull(body.Token);
            Assert.NotEmpty(body.Token);
        }
    }
}